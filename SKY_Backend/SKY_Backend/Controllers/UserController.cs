using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;
using Service.UserService.UserService;
using System.Security.Claims;
using System.Text;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration, ITokenService tokenService)
        {
            _userService = userService;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin([FromBody]UserLoginDTO login)
        {
            try
            {
                if (login.UserName != null && login.Password != null)
                {
                    var result = _userService.UserLogin(login);

                    if (result != null)
                    {
                        var createdToken = _tokenService.CreateJWTToken(result);
                        return Ok(new { token = createdToken, user = result });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("UserRegister")]
        public IActionResult UserRegister([FromBody] UserRegisterDTO register)
        {
            try
            {
                _userService.UserRegister(register);
                return Ok();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("AdminRegister")]
        public IActionResult AdminRegister([FromBody] UserRegisterDTO register)
        {
            try
            {
                _userService.AdminRegister(register);
                return Ok();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
