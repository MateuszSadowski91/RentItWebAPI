using AutoMapper;
using Flurl.Http;
using Newtonsoft.Json;
using RentItAPI.Entities;
using RentItAPI.Exceptions;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public InvoiceService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Create(InvoiceModel model)
        {
            var postResult = await "https://invoice-generator.com"
            .WithHeader("content-type", "application/json")
            .PostJsonAsync(model);

            if(!postResult.ResponseMessage.IsSuccessStatusCode)
            {
                throw new ExternalServerError("Something went wrong in an external server, outside of this application.");
            }

            var rootPath = Directory.GetCurrentDirectory();
            var fullPath = $"{rootPath}\\wwwroot\\Invoices\\{DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss")}-Invoice.pdf";

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await postResult.ResponseMessage.Content.CopyToAsync(stream);
            }
        }

        public void DeleteInvoice()
        {
            throw new NotImplementedException();
        }
    }
}

    


