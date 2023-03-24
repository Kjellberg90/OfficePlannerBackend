using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : ControllerBase
    {

        private readonly IRoomService _roomService;

        public AdminRoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminOverview/{date}")]
        public IActionResult AdminOverview(string date)
        {
            try
            {
                var result = _roomService.AdminRoomsOverview(date);
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
                var result = _roomService.AdminGetRooms();
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
                _roomService.AdminDeleteRoom(adminDeleteRoom);
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
                _roomService.AdminPostRoom(adminAddRoom);
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
                _roomService.UpdateRoom(roomId, adminEditRoom);
                return Ok();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
