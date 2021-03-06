﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FileManager.Core.Models;
using System.IO;

namespace FileManager.Core.Services
{
    public interface IDirectoryService
    {
        Task<DirectoryClass> _getNames();
        Task<DirectoryClass> _getNames(string path);
        Task<DirectoryClass> _getInfo();
        Task<DirectoryClass> _getInfo(string path);
        Task<DirectoryInfo> _getDirectory(string path);
    }
}
