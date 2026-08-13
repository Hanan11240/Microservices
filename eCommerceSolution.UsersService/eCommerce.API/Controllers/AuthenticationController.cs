using eCommerce.Core.DTO;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]  RegisterRequest register) {
            if (register == null) {
                return BadRequest("Invalid registration data");
            }
           AuthenticationResponse? authenticationResponse =  await _userService.Register(register);
            if(authenticationResponse is null || authenticationResponse.Success == false)
            {
                return BadRequest(authenticationResponse);
            }

            return Ok(authenticationResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]  LoginRequest loginRequest) {
            if(loginRequest is null)
            {
                return BadRequest("Invalid login data");

            }

            AuthenticationResponse?  authenticationResponse = await _userService.Login(loginRequest);

            if(authenticationResponse is null || authenticationResponse.Success == false)
            {
                return Unauthorized(authenticationResponse);
            }

            return Ok(authenticationResponse);
        }
    }
}
