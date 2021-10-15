using RentItAPI.Models;

namespace RentItAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);

        // void ChangeAccountDetails()
        string GenerateJWT(LoginDto dto);
    }
}