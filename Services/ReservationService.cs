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
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IEmailSender _emailSender;
        public ReservationService(AppDbContext dbContext, IMapper mapper, IUserContextService userContextService, IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userContextService = userContextService;
            _emailSender = emailSender;
        }
        public int MakeReservation(int itemId, MakeReservationDto dto)
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

            var reservationEntity = _mapper.Map<Reservation>(dto);
            reservationEntity.ItemId = itemId;
            reservationEntity.FirstName = user.FirstName;
            reservationEntity.LastName = user.LastName;
            reservationEntity.Email = user.Email;
            reservationEntity.CreatedById = _userContextService.GetUserId;
            _dbContext.Add(reservationEntity);
            _dbContext.SaveChanges();

            var emailDto = _mapper.Map<ReservationConfirmationEmailDto>(reservationEntity);
            _emailSender.SendReservationConfirmationEmail(emailDto);
            _emailSender.SendReservationNotificationEmail(emailDto);

            return reservationEntity.Id;
        }
        public PagedResult<GetReservationDto> GetAll(ReservationQuery query)
        {
            var baseQuery = _dbContext
                .Reservations.Where(r => r.ItemId == _userContextService.GetUserId)
                .Include(r => r.Item)
                .Where(r => query.SearchPhrase == null || (r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.LastName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.Item.Name.ToLower().Contains(query.SearchPhrase.ToLower())))));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Reservation, object>>>
                {
                    { nameof(Reservation.FirstName), r => r.FirstName },
                    { nameof(Reservation.LastName), r => r.LastName },
                    { nameof(Reservation.Item.Name), r => r.Item.Name },
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
                var reservationsDtos = _mapper.Map<List<GetReservationDto>>(reservations);
                var result = new PagedResult<GetReservationDto>(reservationsDtos, totalElementsCount, query.PageSize, query.PageNumber);
                return result;
        }

        public PagedResult<GetReservationDto> GetAllForBusiness(ReservationQuery query, int businessId)
        {
            var baseQuery = _dbContext
                .Reservations.Where(r => r.Business.Id== businessId)
                .Include(r => r.Item)
                .Where(r => query.SearchPhrase == null || (r.FirstName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.LastName.ToLower().Contains(query.SearchPhrase.ToLower())
                                                       || (r.Item.Name.ToLower().Contains(query.SearchPhrase.ToLower())))));
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Reservation, object>>>
                {
                    { nameof(Reservation.FirstName), r => r.FirstName },
                    { nameof(Reservation.LastName), r => r.LastName },
                    { nameof(Reservation.Item.Name), r => r.Item.Name },
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
            var reservationsDtos = _mapper.Map<List<GetReservationDto>>(reservations);
            var result = new PagedResult<GetReservationDto>(reservationsDtos, totalElementsCount, query.PageSize, query.PageNumber);
            return result;
        }
    }
}

            

            
    

    

