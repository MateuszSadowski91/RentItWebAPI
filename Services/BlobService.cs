using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using RentItAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _configuration;
        public BlobService(BlobServiceClient blobServiceClient, IConfiguration configuration)
        {
            _blobServiceClient = blobServiceClient;
            _configuration = configuration;
        }

        public async Task DeleteInvoiceBlobAsync(string name)
        {
            var containerClient = GetContainerClient();
            var blobClient = containerClient.GetBlobClient(name);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<BlobInformation> GetInvoiceBlobAsync(string name)
        {
            var containerClient = GetContainerClient();
            var blobClient = containerClient.GetBlobClient(name);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new BlobInformation(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task<IEnumerable<string>> ListInvoiceBlobsAsync(string taxNumber)
        {
            var containerClient = GetContainerClient();
            var items = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                if (blobItem.Name.StartsWith(taxNumber))
                {
                    items.Add(blobItem.Name);
                }
            }
            return items;
        }

        public async Task UploadHttpContentAsync(HttpContent httpContent, string fileName)
        {
            var containerClient = GetContainerClient();
            var blobClient = containerClient.GetBlobClient(fileName);

            await httpContent.LoadIntoBufferAsync();
            await using var stream = (MemoryStream)await httpContent.ReadAsStreamAsync();
            {
                await blobClient.UploadAsync(stream);
            }
        }
        private BlobContainerClient GetContainerClient()
        {
            var containerName = _configuration.GetValue<string>("BlobContainer");
            var client = _blobServiceClient.GetBlobContainerClient(containerName);
            return client;
        }
    }
}