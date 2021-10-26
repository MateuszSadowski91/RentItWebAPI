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
using System.Threading.Tasks;

namespace RentItAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _dbcontext;
        private readonly IPasswordHasher<User> _passwordhasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IEmailSender _emailsender;
        private readonly IUserContextService _usercontextservice;

        public AccountService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings,
           IEmailSender emailSender, IUserContextService userContextService)
        {
            _dbcontext = dbContext;
            _passwordhasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _emailsender = emailSender;
            _usercontextservice = userContextService;
        }

        public string GenerateJWT(LoginDto dto)
        {
            var user = _dbcontext.Users
                 .Include(u => u.Role)
                 .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
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
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
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

        public void GrantAdminRole(GrantAdminRoleDto dto)
        {
            User user = GetUser(dto);

            user.RoleId = dto.GrantedRoleId;
            _dbcontext.SaveChanges();
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            IsPasswordConfirmed(dto);
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

        public async Task ResetPassword(ResetPasswordDto dto)
        {
            var user = GetUser(dto);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, $"{user.Email}")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var resetPasswordToken = tokenHandler.WriteToken(token);

            var emailDto = new ResetPasswordEmailDto();
            emailDto.EmailAddress = dto.EmailAddress;
            emailDto.ResetToken = resetPasswordToken;

            await _emailsender.SendResetPasswordEmail(emailDto);
        }

        private User GetUser(ResetPasswordDto dto)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Email == dto.EmailAddress);
            if (user == null)
            {
                throw new NotFoundException("User was not found. Please make sure that inserted data is correct.");
            }

            return user;
        }

        private User GetUser(GrantAdminRoleDto dto)
        {
            var user = _dbcontext.Users.FirstOrDefault(u => u.Email == dto.UserEmail);
            if (user == null)
            {
                throw new NotFoundException("User was not found. Please make sure that inserted data is correct.");
            }

            return user;
        }

        public void SetNewPassword(SetNewPasswordDto dto)
        {
            IsPasswordConfirmed(dto);
            var user = _dbcontext.Users.FirstOrDefault(u => u.Id == _usercontextservice.GetUserId);
            if (user == null)
            {
                throw new NotFoundException("User not found. Please make sure that inserted data is correct.");
            }

            var hashedPassword = _passwordhasher.HashPassword(user, dto.Password);
            user.PasswordHash = hashedPassword;
            _dbcontext.SaveChanges();
        }

        private static void IsPasswordConfirmed(RegisterUserDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new BadRequestException("Values in fields: Password and Confirm Password must be same.");
            }
        }

        private static void IsPasswordConfirmed(SetNewPasswordDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new BadRequestException("Values in fields: Password and Confirm Password must be same.");
            }
        }
    }
}