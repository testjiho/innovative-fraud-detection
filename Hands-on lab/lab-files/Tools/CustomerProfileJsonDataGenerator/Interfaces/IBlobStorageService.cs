using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CustomerProfileJsonDataGenerator.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> GetFileContentAsString(string containerName, string filePath);

        Task SetFileContentAsString(string containerName, string filePath, string content);
        Task SetFileContentAsStream(string containerName, string filePath, Stream content);
    }
}
