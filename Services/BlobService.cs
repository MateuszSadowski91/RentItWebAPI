using Azure.Storage.Blobs;
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

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task DeleteInvoiceBlobAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("invoice");
            var blobClient = containerClient.GetBlobClient(name);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task<BlobInformation> GetInvoiceBlobAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("invoice");
            var blobClient = containerClient.GetBlobClient(name);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            return new BlobInformation(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
        }

        public async Task<IEnumerable<string>> ListInvoiceBlobsAsync(string taxNumber)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("invoice");
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
            var containerClient = _blobServiceClient.GetBlobContainerClient("invoice");
            var blobClient = containerClient.GetBlobClient(fileName);

            await httpContent.LoadIntoBufferAsync();
            await using var stream = (MemoryStream)await httpContent.ReadAsStreamAsync();
            {
                await blobClient.UploadAsync(stream);
            }
        }
    }
}