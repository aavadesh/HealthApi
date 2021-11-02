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
            string token = _accountService.GenerateJwt(dto);

            response = Ok(new { token }); 
            return response;
        }
    }
}
