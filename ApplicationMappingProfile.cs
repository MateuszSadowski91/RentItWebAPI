using AutoMapper;
using RentItAPI.Entities;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI
{
    public class ApplicationMappingProfile: Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<CreateBusinessDto, Business>();
            CreateMap<CreateItemDto, Item>();
            CreateMap<Business, GetBusinessDto>();
            CreateMap<Item, GetItemDto>();
            CreateMap<MakeReservationDto, Reservation>();
            CreateMap<Reservation, GetReservationDto>();
            CreateMap<MakeRequestDto, Request>();
            CreateMap<Request, GetRequestDto>();
            CreateMap<Request, Reservation>();
            CreateMap<Request, RequestEmailDto>();
            CreateMap<Item, RequestEmailDto>()
                .ForMember(d => d.ItemName, a => a.MapFrom(s => s.Name));
            CreateMap<Request, ReservationConfirmationEmailDto>();
            CreateMap<Item, ReservationConfirmationEmailDto>()
                .ForMember(d => d.ItemName, a => a.MapFrom(s => s.Name));


        }
    }
}

