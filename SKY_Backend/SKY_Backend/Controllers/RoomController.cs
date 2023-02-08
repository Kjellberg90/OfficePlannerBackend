using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("get-rooms-info")]
        public IActionResult GetRoomsInfo(string date)
        {
            try
            {
                var result = _roomService.GetRoomsInfo(date);
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
    }
}
