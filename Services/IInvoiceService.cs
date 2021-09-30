using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IInvoiceService
    {
        Task Create(InvoiceModel model);
        void DeleteInvoice();
    }
}
