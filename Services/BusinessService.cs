using AutoMapper;
using RentItAPI.Entities;
using RentItAPI.Exceptions;
using RentItAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RentItAPI.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public BusinessService(IMapper mapper, IUserContextService userContextService, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public void Delete(int businessId)
        {
            var business = _dbContext.Businesses.FirstOrDefault(b => b.Id == businessId);
            var userId = _userContextService.GetUserId;
            if (userId != business.CreatedById)
            {
                throw new AccessForbiddenException("You do not have a permission to access this resource.");
            }

            _dbContext.Remove(business);
        }

        public IEnumerable<GetBusinessDto> GetAll()
        {
            var businesses = _dbContext.Businesses.ToList();
            var result = _mapper.Map<List<GetBusinessDto>>(businesses);

            return result;
        }

        public GetBusinessDto GetById(int businessId)
        {
            var business = _dbContext.Businesses.FirstOrDefault(b => b.Id == businessId);
            var result = _mapper.Map<GetBusinessDto>(business);

            return result;
        }

        public int Create(CreateBusinessDto dto)
        {
            var business = _mapper.Map<Business>(dto);
            business.CreatedById = _userContextService.GetUserId;
            _dbContext.Businesses.Add(business);
            _dbContext.SaveChanges();

            return business.Id;
        }
    }
}