using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentItAPI.Entities;
using RentItAPI.Exceptions;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RentItAPI.Services
{
    public class RequestService : IRequestService
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        public RequestService(AppDbContext dbContext, IUserContextService userContextService, IMapper mapper, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _userContextService = userContextService;
            _mapper = mapper;
            _emailSender = emailSender;
        }
        public void AcceptRequest(int requestId, string? message)
        {
            var request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            if (request.CreatedById != _userContextService.GetUserId || request is null)
                throw new NotFoundException($"Cannot accept a request of ID: ({requestId}) Please check if the ID is correct.");

            request.RequestStatus = Status.Accepted;

            var newReservation = _mapper.Map<Reservation>(request);
            _dbContext.Add(newReservation);
            _dbContext.SaveChanges();

            var requestReplyDto = _mapper.Map<ReservationStatusEmailDto>(request);
            var item = request.Item;
            _mapper.Map(item, requestReplyDto);
            requestReplyDto.ReplyMessage = message;
            requestReplyDto.ReservationStatus = request.RequestStatus.ToString();

            _emailSender.SendReservationStatusEmail(requestReplyDto);
        }
        public void RejectRequest(int requestId, string? reason)
        {
            var request = _dbContext.Requests.FirstOrDefault(r => r.Id == requestId);
            if (request.CreatedById != _userContextService.GetUserId || request is null)
               throw new NotFoundException($"Cannot reject a request of ID: ({requestId}) Please check if the ID is correct.");

            request.RequestStatus = Status.Rejected;

            var emailDto = _mapper.Map<ReservationStatusEmailDto>(request);
            var item = request.Item;
            _mapper.Map(item, emailDto);
            emailDto.ReplyMessage = reason;
            emailDto.ReservationStatus = request.RequestStatus.ToString();

            _emailSender.SendReservationStatusEmail(emailDto);
        }
        public PagedResult<GetRequestDto> GetAll(RequestQuery query)
        {
            var baseQuery = _dbContext
                .Requests.Where(r => r.ItemId == _userContextService.GetUserId)
                .Include(r => r.Item)
                .Where(r => query.SearchPhrase == null || (r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.LastName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.Item.Name.ToLower().Contains(query.SearchPhrase.ToLower())))));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Request, object>>>
                {
                    { nameof(Request.FirstName), r => r.FirstName },
                    { nameof(Request.LastName), r => r.LastName },
                    { nameof(Request.Item.Name), r => r.Item.Name },
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
            var reservationsDtos = _mapper.Map<List<GetRequestDto>>(reservations);
            var result = new PagedResult<GetRequestDto>(reservationsDtos, totalElementsCount, query.PageSize, query.PageNumber);
            return result;
        }
        public PagedResult<GetRequestDto> GetAllForBusiness(RequestQuery query, int businessId)
        {
            var baseQuery = _dbContext
                .Requests
                .Where(r => r.Business.Id == businessId)
                .Include(r => r.Item)
                .Where(r => query.SearchPhrase == null || (r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.LastName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.Item.Name.ToLower().Contains(query.SearchPhrase.ToLower())))));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Request, object>>>
                {
                    { nameof(Request.FirstName), r => r.FirstName },
                    { nameof(Request.LastName), r => r.LastName },
                    { nameof(Request.Item.Name), r => r.Item.Name },
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
            var reservationsDtos = _mapper.Map<List<GetRequestDto>>(reservations);
            var result = new PagedResult<GetRequestDto>(reservationsDtos, totalElementsCount, query.PageSize, query.PageNumber);
            return result;
        }
        public int MakeRequest(int itemId, MakeRequestDto dto)
        {
            var reservations = _dbContext.Reservations.Where(r => r.ItemId == itemId);
            foreach (var reservation in reservations)
            {
                if (dto.DateTo >= reservation.DateFrom && dto.DateTo <= reservation.DateTo
                || dto.DateFrom >= reservation.DateFrom && dto.DateFrom <= reservation.DateTo
                || dto.DateFrom <= reservation.DateFrom && dto.DateTo >= reservation.DateTo)
                {
                    throw new BadRequestException("Inserted dates collide with dates of existing reservations.");
                }
            }
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == _userContextService.GetUserId);

            var newRequestEntity = _mapper.Map<Request>(dto);
            newRequestEntity.ItemId = itemId;
            newRequestEntity.FirstName = user.FirstName;
            newRequestEntity.LastName = user.LastName;
            newRequestEntity.Email = user.Email;
            newRequestEntity.CreatedById = _userContextService.GetUserId;
            _dbContext.Add(newRequestEntity);
            _dbContext.SaveChanges();


            var emailDto = _mapper.Map<RequestEmailDto>(newRequestEntity);
            var item = _dbContext.Items.FirstOrDefault(i => i.Id == itemId);
            _mapper.Map(item, emailDto);

            _emailSender.SendRequestNotificationEmail(emailDto);
            _emailSender.SendRequestConfirmationEmail(emailDto);

            return newRequestEntity.Id;
        } 
    }
}
