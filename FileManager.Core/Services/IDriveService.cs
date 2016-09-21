using System.Collections.Generic;
using System.Threading.Tasks;
using FileManager.Core.Models;
using System.IO;


namespace FileManager.Core.Services
{
    public interface IDriveService
    {
        Task <List<Drive>> GetAllDrives();
        Task<Drive> GetDrive(DriveInfo driveInfo);
    }

}
