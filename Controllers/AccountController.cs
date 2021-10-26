using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentItAPI.Models;
using RentItAPI.Services;
using System.Threading.Tasks;

namespace RentItAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok(dto);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.GenerateJWT(dto);
            return Ok(token);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("grant")]
        public ActionResult GrantAdminRole([FromBody] GrantAdminRoleDto dto)
        {
            _accountService.GrantAdminRole(dto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _accountService.ResetPassword(dto);
            return Ok();
        }

        [Authorize]
        [HttpPost("setpassword")]
        public ActionResult SetNewPassword([FromBody] SetNewPasswordDto dto)
        {
            _accountService.SetNewPassword(dto);
            return Ok();
        }
    }
}