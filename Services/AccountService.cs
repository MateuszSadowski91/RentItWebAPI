using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentItAPI.Entities;
using RentItAPI.Exceptions;
using RentItAPI.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace RentItAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _dbcontext;
        private readonly IPasswordHasher<User> _passwordhasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            _dbcontext = dbContext;
            _passwordhasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public string GenerateJWT(LoginDto dto)
        {
            var user = _dbcontext.Users
                 .Include(u => u.Role)
                 .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password.");
            }

            var loginresult = _passwordhasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (loginresult == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey)); //przekazanie klucza prywatnego jako tablicy bajtów.
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            if (dto.Password == dto.ConfirmPassword)
            {
                var newUser = new User
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    DateOfBirth = dto.DateOfBirth,
                    AccountNumber = dto.AccountNumber,
                    RoleId = dto.RoleId,
                };
                var hashedPassword = _passwordhasher.HashPassword(newUser, dto.Password);
                newUser.PasswordHash = hashedPassword;
                _dbcontext.Users.Add(newUser);
                _dbcontext.SaveChanges();
            }
        }
    }
}