using FileManager.Core.Models;
using FileManager.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManager.Services.Services
{
    public class DirectoryService : IDirectoryService, IDriveService
    {
        private string _path;
        private DirectoryClass directory;
        private DirectoryInfo directoryInfo;
        public static long Mb = 1000000;

        public DirectoryService()
        {
            directory = new DirectoryClass();

            //getting Logical Drives
            directory.Drives = _getDrives();
        }

        public async Task<DirectoryClass> _get(string path)
        {
            if (String.IsNullOrEmpty(path) || String.IsNullOrWhiteSpace(path))
            {
                return await _get();
            }
            return await _ProcessDirectory(path);
        }
        public async Task<DirectoryClass> _ProcessDirectory(string path)
        {
            directoryInfo = await _getDirectory(path);

            directory.Name = directoryInfo.Name;
            directory.path = new PathClass { path = path, parrentPath = (directoryInfo.Parent == null) ? path : directoryInfo.Parent.FullName, rootPath = directoryInfo.Root.FullName };
            //getting directory.Directories
            directory.Folders = await GetSubDirectories(directoryInfo, SearchOption.TopDirectoryOnly);
            //directory.Files
            directory.Files = await GetDirFiles(directoryInfo, SearchOption.TopDirectoryOnly);

            directory.Size = Math.Round(directory.Folders.Sum(q => q.Size) + directory.Files.Sum(q => q.Size), 2);
            directory.Created = directoryInfo.CreationTime;
            directory.Modified = directoryInfo.LastWriteTime;
            directory.GetCurrentDrive();
            await SortItems();

            return directory;
        }

        public async Task<DirectoryInfo> _getDirectory(string path)
        {
            return await Task.Run(() => {
                var dir = new DirectoryInfo(path);
                if (dir.Exists)
                    return dir;
                return new DirectoryInfo(Directory.GetCurrentDirectory());
            }
            );
        }

        public static async Task<List<SubDirectoryClass>> GetSubDirectories(DirectoryInfo d, SearchOption searchOption)
        {
            return await Task.Run(async () =>
            {
                var subDirs = d.EnumerateDirectories("*", searchOption).Select(q => new SubDirectoryClass
                {
                    path = new PathClass { path = q.FullName },
                    Name = q.Name,
                    FullName = q.FullName,
                    Created = q.CreationTime,
                    Modified = q.LastWriteTime
                }).ToList();
                foreach (var folder in subDirs)
                {
                    try {
                        //    var alldirFiles = await GetAllDirFiles(folder.FullName, SearchOption.AllDirectories);
                        var alldirFiles = await GetDirFilesSizes(new DirectoryInfo(folder.FullName));

                        folder.category = new CountCategory { };
                        folder.category.totalSmallItems = alldirFiles.Where(q => q < (Mb * 10)).Count();
                        folder.category.totalMediumItems = alldirFiles.Where(q => q >= (Mb * 10) && q <= (Mb * 50)).Count();
                        folder.category.totalBigItems = alldirFiles.Where(q => q > (Mb * 100)).Count();
                        folder.Size = ConvertBytesToMegabytes(alldirFiles.Sum());
                    }
                    catch(Exception ex){ }
                }
                return subDirs;
            });
        }

    
        public static async Task<List<FileClass>> GetDirFiles(DirectoryInfo d, SearchOption searchOption)
        {
            return await Task.Run(() =>
            {
                var files = new List<FileClass>();
                try
                {
                    foreach (var file in d.EnumerateFiles("*", searchOption))
                    {
                        try { files.Add(new FileClass { Size = ConvertBytesToMegabytes(file.Length), Name = file.Name }); }
                        catch { files.Add(new FileClass { Size = 0, Name = file.Name }); }
                    }
                }
                catch(Exception ex)
                {
                }
                return files;
            });
        }

        public static async Task<List<long>> GetDirFilesSizes(DirectoryInfo d)
        {
            List<long> Sizes = new List<long>();
            try
            {
                // Add subdirectory sizes.
                var Enumfiles = d.EnumerateFiles("*", SearchOption.TopDirectoryOnly);
                foreach (FileInfo file in Enumfiles)
                {
                    try
                    {
                        Sizes.Add(file.Length);
                    }
                    catch (Exception ex) { }
                }
            }
            catch(Exception ex) { }
            try
             {
                 // Add subdirectory sizes.
                 var Enumdirs = d.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
                 foreach (DirectoryInfo dir in Enumdirs)
                 {
                     try
                     {
                        Sizes.AddRange(await GetDirFilesSizes(dir));
                     }
                    catch (Exception ex) { }
                }
             }
            catch (Exception ex) { }
            // Add file sizes.
            return Sizes;
        }
        public async Task SortItems()
        {
            await Task.Run(() => {
                directory.category = new CountCategory { };
                try { directory.category.totalSmallItems = directory.Files.Where(q => q.Size < 10).Count() + directory.Folders.Sum(q => q.category.totalSmallItems); } catch { }
                try { directory.category.totalMediumItems = directory.Files.Where(q => q.Size >= 10 && q.Size <= 50).Count() + directory.Folders.Sum(q => q.category.totalMediumItems); } catch (Exception ex) { }
                try { directory.category.totalBigItems = directory.Files.Where(q => q.Size > 100).Count() + directory.Folders.Sum(q => q.category.totalBigItems); } catch (Exception ex) { }
            });
        }


        public List<Drive> _getDrives()//getting Logical Drives
        {
            var pcDrives = Task.Run(async () => {
                return await GetAllDrives();
            });
            return pcDrives.Result;
        }
        public async Task<List<Drive>> GetAllDrives()
        {
            return await Task.Run(async () =>
            {
                var DrivesList = new List<Drive>();
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (var drive in allDrives)
                {
                    if (drive.IsReady)
                        try
                        {
                            Drive hardDisk = await GetDrive(drive);
                            DrivesList.Add(hardDisk);
                        }
                        catch (Exception ex) { }
                }
                return DrivesList;
            });
        }
        public async Task<Drive> GetDrive(DriveInfo driveInfo)
        {
            return await Task.Run(() => {
                var drive = new Drive();
                drive.dAvailableFreeSpace = driveInfo.AvailableFreeSpace;
                drive.dType = driveInfo.DriveType;
                drive.dFormat = driveInfo.DriveFormat;
                drive.dName = new PathClass { rootPath = driveInfo.Name };
                drive.dTotalFreeSpace = driveInfo.TotalFreeSpace;
                drive.dTotalSize = driveInfo.TotalSize;
                drive.dVolumeLabel = driveInfo.VolumeLabel;
                drive.iScurrent = false;
                return drive;
            });
        }

        public async Task<DirectoryClass> _get()
        {
            _path = Directory.GetCurrentDirectory();
            return await _ProcessDirectory(_path);
        }
        static double ConvertBytesToMegabytes(long bytes)
        {
            long B = 0, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size = 0;
            size = Math.Round((double)bytes / MB, 2);
            return size;
        }




        /*   public static async Task<IEnumerable<long>> GetAllDirFiles(string path, SearchOption searchOption)
   {
       return await Task.Run(() =>
       {

             // var allFiles = Directory.EnumerateFiles(path, "*.*", searchOption);
           var alldirFiles  = GetAllFiles(path, "*");

              var fileQ = from file in alldirFiles select getFileSize(file);
              return fileQ;//..ToArray();
       });
   }*/
        /*   static long getFileSize(string fileName)
   {
       long fileSize;
       try
       {
           System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
           fileSize = fi.Length;
       }
       catch (System.IO.FileNotFoundException)
       {
           fileSize = 0;
       }
       return fileSize;
   }*/
        /*   public static IEnumerable<String> GetAllFiles(string path, string searchPattern)
           {
               return System.IO.Directory.EnumerateFiles(path,"*").Union(
                   System.IO.Directory.EnumerateDirectories(path).SelectMany(d =>
                   {
                       try
                       {
                           return GetAllFiles(d, "*");
                       }
                       catch (UnauthorizedAccessException e)
                       {
                           return Enumerable.Empty<String>();
                       }
                   }));
           }*/



    }
}
