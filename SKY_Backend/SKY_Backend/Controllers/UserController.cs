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
        public IActionResult UserLogin([FromBody]LoginDTO login)
        {
            try
            {
                //var user = new UserController();

                var result = _userService.UserLogin(login);
                //if (result == null)
                //{
                //    return NotFound();
                //}
                return Ok(result);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
