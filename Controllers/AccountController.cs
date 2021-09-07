using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Entities;
using RentItAPI.Models;
using RentItAPI.Services;
using Restaurantapi2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentItAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountservice;
        public AccountController(IAccountService accountService)
        {
            _accountservice = accountService;
        }
     
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountservice.RegisterUser(dto);
            return Ok(dto);
        }
        
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountservice.GenerateJWT(dto);
            return Ok(token);
        }
    }
}
