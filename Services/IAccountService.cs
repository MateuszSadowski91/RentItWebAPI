using RentItAPI.Entities;
using RentItAPI.Models;
using Restaurantapi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        // void ChangeAccountDetails()
        string GenerateJWT(LoginDto dto);
    }
}
