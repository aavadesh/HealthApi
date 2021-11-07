using HealthApi.Entities;
using HealthApi.Exceptions;
using HealthApi.Models;
using HealthApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("signup")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            IActionResult response = Unauthorized();
            User user = _accountService.AuthenticateUser(dto);
            if (user == null)
            {
                throw new BadRequestException("Invalid username or password");
            }
            var tokenString = _accountService.GenerateJwt(user, dto.Password);
            response = Ok(new
            {
                token = tokenString,
                userDetails = user,
            });
            return response;
        }
    }
}
