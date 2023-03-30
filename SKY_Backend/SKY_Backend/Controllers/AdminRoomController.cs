using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.AdminRomService;
using Service.DTO;
using Service.RoomService;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : ControllerBase
    {

        private readonly IAdminRoomService _adminRoomService;

        public AdminRoomController(IAdminRoomService adminRoomService)
        {
            _adminRoomService = adminRoomService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminOverview/{date}")]
        public IActionResult AdminOverview(string date)
        {
            try
            {
                var result = _adminRoomService.AdminRoomsOverview(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminGetRooms")]
        public IActionResult AdminGetRooms()
        {
            try
            {
                var result = _adminRoomService.AdminGetRooms();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("adminDeleteRooms")]
        public IActionResult AdminDeleteRoom([FromBody] AdminDeleteRoomDTO adminDeleteRoom)
        {
            try
            {
                _adminRoomService.AdminDeleteRoom(adminDeleteRoom);
                return Ok(adminDeleteRoom);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("adminAddRooms")]
        public IActionResult AdminPostRoom([FromBody] AdminPostRoomDTO adminAddRoom)
        {
            try
            {
                _adminRoomService.AdminPostRoom(adminAddRoom);
                return Ok();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.StackTrace);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("adminroomEditRoom")]
        public IActionResult AdminEditRoom(int roomId, [FromBody] AdminEditRoomDTO adminEditRoom)
        {
            try
            {
                _adminRoomService.UpdateRoom(roomId, adminEditRoom);
                return Ok();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
