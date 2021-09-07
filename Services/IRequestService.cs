using RentItAPI.Entities;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IRequestService
    {
        int MakeRequest(int itemId, MakeRequestDto dto);
        PagedResult<GetRequestDto> GetAll (RequestQuery query);
        PagedResult<GetRequestDto> GetAllForBusiness (RequestQuery query, int businessId);
        void AcceptRequest(int requestId, string? message);
        void RejectRequest(int requestId, string? reason);
    }
}
