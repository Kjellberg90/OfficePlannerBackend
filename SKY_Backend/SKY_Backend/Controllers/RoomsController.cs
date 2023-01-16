using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        [HttpGet("get-rooms")]
        public IActionResult GetRooms()
        {
            IRoomService roomService = new RoomService();

            try
            {
                var result = roomService.GetRooms();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-room")]
        public IActionResult GetRoom(int roomId)
        {
            IRoomService roomService = new RoomService();
            
            try
            {
                var result = roomService.GetRoom(roomId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
