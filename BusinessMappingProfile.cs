using AutoMapper;
using RentItAPI.Entities;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI
{
    public class BusinessMappingProfile: Profile
    {
        public BusinessMappingProfile()
        {
            CreateMap<CreateBusinessDto, Business>();
            CreateMap<CreateItemDto, Item>();
            CreateMap<Business, GetBusinessDto>();
            CreateMap<Item, GetItemDto>();
            CreateMap<MakeReservationDto, Reservation>();
        }
    }
}
