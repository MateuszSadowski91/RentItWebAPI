using Flurl.Http;
using RentItAPI.Entities;
using RentItAPI.Exceptions;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _dbContext;
        private readonly IBlobService _blobService;
        private readonly IUserContextService _userContextService;

        public InvoiceService(AppDbContext dbContext, IBlobService blobService, IUserContextService userContextService)
        {
            _blobService = blobService;
            _dbContext = dbContext;
            _userContextService = userContextService;
        }
        public async Task<List<string>> GetListAsync(int businessId)
        {
            var business = GetBusiness(businessId);
            ValidateInput(business);
            var taxNumber = business.TaxNumber;
            var invoiceList = await _blobService.ListInvoiceBlobsAsync(taxNumber);

            return invoiceList.ToList();
        }
        public async Task <BlobInformation> GetAsync (int businessId, string fileName)
        {
            var business = GetBusiness(businessId);
            ValidateInput(business);
            var invoice = await _blobService.GetInvoiceBlobAsync(fileName);

            return invoice;
        }
        public async Task CreateAsync(int businessId, InvoiceModel model)
        {
            var business = GetBusiness(businessId);
            ValidateInput(business);
            var postResult = await "https://invoice-generator.com"
            .WithHeader("content-type", "application/json")
            .PostJsonAsync(model);

            if (!postResult.ResponseMessage.IsSuccessStatusCode)
            {
                throw new ExternalServerError("Something went wrong in an external server, outside of this application.");
            }

            var fileName = $"{business.TaxNumber}-{business.Name}-{DateTime.Now:yyyy-dd-M-HH-mm-ss}.pdf";
            await _blobService.UploadHttpContentAsync(postResult.ResponseMessage.Content, fileName);
        }
        public async Task DeleteAsync(int businessId, DeleteInvoiceDto dto)
        {
            var business = GetBusiness(businessId);
            var namesToDelete = dto.FileNames;
            var invoicesOnServer = await _blobService.ListInvoiceBlobsAsync(business.TaxNumber);

            ValidateInput(business, namesToDelete, invoicesOnServer);

            foreach (var name in namesToDelete)
            {
                await _blobService.DeleteInvoiceBlobAsync(name);
            }
        }
        private Business GetBusiness(int businessId)
        {
            var business = _dbContext.Businesses.FirstOrDefault(b => b.Id == businessId);
            if (business == null || business.CreatedById != _userContextService.GetUserId)
            {
                throw new NotFoundException("Business not found. Please check the inserted values.");
            }
            return business;
        }
        private static void ValidateInput(Business business, List<string> namesToDelete, IEnumerable<string> invoicesOnServer)
        {
           
            foreach (var name in namesToDelete)
            {
                var taxMatchesPrefix = name.StartsWith(business.TaxNumber);
                if (!taxMatchesPrefix)
                {
                    throw new NotFoundException("Some files were not found. Please check the inserted names.");
                }
            }
            var listOfInvoices = invoicesOnServer.ToList();

            foreach (var name in listOfInvoices)
            {
                if(!listOfInvoices.Contains(name))
                throw new NotFoundException("Some files were not found. Please check the inserted names.");
            }
        }
        private void ValidateInput(Business business)
        {
            if (business.CreatedById != _userContextService.GetUserId)
            {
                throw new AccessForbiddenException("You do not have a permission to access this resource.");
            }
        }
    }
}


    


