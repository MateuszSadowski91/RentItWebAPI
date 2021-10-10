using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IInvoiceService
    {
        Task CreateAsync(int businessId, InvoiceModel model);
        Task DeleteAsync(int businessId, DeleteInvoiceDto dto);
        Task<BlobInformation> GetAsync (int businessId, string fileName);
        Task <List<string>> GetListAsync(int businessId);
    }
}
