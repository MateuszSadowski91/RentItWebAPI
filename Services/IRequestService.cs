using RentItAPI.Models;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IRequestService
    {
        Task<int> MakeRequest(int itemId, MakeRequestDto dto);

        PagedResult<GetRequestDto> GetAll(RequestQuery query);

        PagedResult<GetRequestDto> GetAllForBusiness(RequestQuery query, int businessId);

        Task AcceptRequest(int requestId, string? message);

        Task RejectRequest(int requestId, string? reason);
    }
}