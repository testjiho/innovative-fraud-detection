using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CustomerProfileJsonDataGenerator.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace CustomerProfileJsonDataGenerator.Storage
{
    public class AzureBlobStorageService : IBlobStorageService
    {
        AzureBlobStorageServiceSettings _settings;
        CloudBlobClient _blobClient;

        public AzureBlobStorageService(
            IOptions<AzureBlobStorageServiceSettings> settings)
        {
            _settings = settings.Value;

            var storageAccount = CloudStorageAccount.Parse(_settings.ConnectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> GetFileContentAsString(string containerName, string filePath)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(filePath);
            return await blob.DownloadTextAsync();
        }

        public async Task SetFileContentAsStream(string containerName, string filePath, Stream content)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(filePath);
            await blob.UploadFromStreamAsync(content);
        }

        public async Task SetFileContentAsString(string containerName, string filePath, string content)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(filePath);
            await using var stream = new MemoryStream(Encoding.Default.GetBytes(content), false);
            await blob.UploadFromStreamAsync(stream);
        }
    }
}
