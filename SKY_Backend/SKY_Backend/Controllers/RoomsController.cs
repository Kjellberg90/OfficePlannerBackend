using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("get-rooms-info")]
        public IActionResult GetRoomsInfo()
        {
            try
            {
                var result = _roomService.GetRoomsInfo();
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

        [HttpGet("get-room")]
        public IActionResult GetRoom(int roomId)
        {
            try
            {
                var result = _roomService.GetRoom(roomId);

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

        [HttpGet("get-rooms")]
        public IActionResult GetRooms()
        {
            try
            {
                var result = _roomService.GetRooms();

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

        [HttpPost("post-room")]
        public IActionResult PostRoom(PostRoomDTO room)
        {
            try
            {
                _roomService.PostRoomToFile(room);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-room")]
        public IActionResult DeleteRoom(int roomId)
        {
            try
            {
                _roomService.DeleteRoom(roomId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-room")]
        public IActionResult UpdateRoom(Room room)
        {
            try
            {
                _roomService.UpdateRoom(room);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
