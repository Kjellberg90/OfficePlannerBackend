using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult UserLogin([FromBody]UserLoginDTO login)
        {
            try
            {
                var result = _userService.UserLogin(login);
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost("Register")]
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

    }
}
