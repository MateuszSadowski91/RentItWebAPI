using AutoMapper;
using RentItAPI.Entities;
using RentItAPI.Models;

namespace RentItAPI
{
    public class ApplicationMappingProfile : Profile
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
            CreateMap<Request, ReservationStatusEmailDto>();
            CreateMap<Item, ReservationStatusEmailDto>()
                .ForMember(d => d.ItemName, a => a.MapFrom(s => s.Name));
            CreateMap<ModifyItemDto, Item>();
        }
    }
}