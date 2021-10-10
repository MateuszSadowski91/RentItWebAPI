using Azure.Storage.Blobs.Models;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
   public interface IBlobService
    {
        public Task<BlobInformation> GetInvoiceBlobAsync(string name);
        public Task<IEnumerable<string>> ListInvoiceBlobsAsync(string taxNumber);
        public Task UploadHttpContentAsync(HttpContent httpContent, string fileName);
        public Task DeleteInvoiceBlobAsync(string blobName);
    }
}
