using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.AdminRomService;
using Service.DTO;
using Microsoft.AspNetCore.Authorization;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : ControllerBase
    {
        private readonly IAdminRoomService _adminRoomService;

        public AdminRoomController(IAdminRoomService adminroomService)
        {
            _adminRoomService = adminroomService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminOverviewFromWeek")]
        public IActionResult AdminOverview([FromQuery] int weekNr, [FromQuery] int scheduleId)
        {
            try
            {
                var result = _adminRoomService.AdminRoomsOverview(weekNr, scheduleId);
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
        [HttpGet("adminOverviewFromDate")]
        public IActionResult AdminOverview([FromQuery] string date, [FromQuery] int scheduleId)
        {
            try
            {
                var result = _adminRoomService.AdminRoomsOverview(date, scheduleId);
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
