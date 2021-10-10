using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentItAPI.Entities;
using RentItAPI.Exceptions;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        public ItemService(AppDbContext dbContext, IMapper mapper, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        public PagedResult<GetItemDto> GetAll(int businessId, ItemQuery query)
        {
            var baseQuery = _dbContext
                .Items.Where(i => i.BusinessId == businessId)

                .Where(i => query.SearchPhrase == null || (i.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (i.Price.ToString().ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (i.Description.ToLower().Contains(query.SearchPhrase.ToLower())))));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Item, object>>>
                {
                    { nameof(Item.Name), i => i.Name },
                    { nameof(Item.Price), i => i.Price },
                    { nameof(Item.Description), i => i.Description },
                };

                var selectedColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                     ? baseQuery.OrderBy(selectedColumn)
                     : baseQuery.OrderByDescending(selectedColumn);
            }
            var reservations = baseQuery
                 .Skip(query.PageSize * (query.PageNumber - 1))
                 .Take(query.PageSize)
                 .ToList();

            var totalElementsCount = baseQuery.Count();
            var itemsDtos = _mapper.Map<List<GetItemDto>>(reservations);
            var result = new PagedResult<GetItemDto>(itemsDtos, totalElementsCount, query.PageSize, query.PageNumber);

            return result;
        }
        public GetItemDto GetById(int businessId, int itemId)
        {
            var item = _dbContext.Items.FirstOrDefault(i => i.Id == itemId);
            if (item is null || item.BusinessId != businessId)
            {
                throw new NotFoundException("Item not found.");
            }
            var itemDto = _mapper.Map<GetItemDto>(item);

            return itemDto;
        }
        public void Delete(int itemId, int businessId)
        {
            var item = _dbContext.Items.FirstOrDefault(i => i.Id == itemId);

            if (item is null || item.BusinessId != businessId || item.Business.CreatedById !=_userContextService.GetUserId)
            {
                throw new NotFoundException("Item not found.");
            }
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
        }
        public int Create(CreateItemDto dto, int businessId)
        {
            var business = _dbContext.Businesses.FirstOrDefault(b => b.Id == businessId);
            if (business.CreatedById != _userContextService.GetUserId)
            {
                throw new NotFoundException($"Cannot create an item for business of ID:({businessId}) Please check if the ID is correct.");
            }

            var itemEntity = _mapper.Map<Item>(dto);
            itemEntity.BusinessId = businessId;
            _dbContext.Items.Add(itemEntity);
            _dbContext.SaveChanges();

            return itemEntity.Id;
        }
        private Business GetBusinessById(int businessId)
        {
            var business = GetBusinessById(businessId);

            if (business is null)
            {
                throw new NotFoundException("Business not found.");
            }
            return business;
        }

        public void Modify(ModifyItemDto dto, int businessId, int itemId)
        {
            var business = _dbContext.Businesses.FirstOrDefault(b => b.Id == businessId);
            var item = _dbContext.Items.FirstOrDefault(i => i.Id == itemId);

            if (item is null || item.BusinessId != businessId || item.Business.CreatedById != _userContextService.GetUserId)
            {
                throw new NotFoundException("Item not found.");
            }

            _mapper.Map<Item>(dto);
            _dbContext.SaveChanges();
        }
    }
}
