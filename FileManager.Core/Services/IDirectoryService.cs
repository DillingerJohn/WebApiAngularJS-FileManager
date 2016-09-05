using System.Collections.Generic;
using System.Threading.Tasks;
using FileManager.Core.Models;
using System.IO;

namespace FileManager.Core.Services
{
    public interface IDirectoryService
    {
        Task<DirectoryClass> _get();
        Task<DirectoryClass> _get(string path);
        Task<DirectoryInfo> _getDirectory(string path);
    }
}
