using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomBookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public AdminRoomBookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateBookings/{date}")]
        public IActionResult UpdateBookings([FromBody] UpdateBookingsDTO[] updateBookingsDTO, string date)
        {
            try
            {
                _bookingService.UpdateBookings(updateBookingsDTO, date);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("getGroupsBookedToRoom")]
        public IActionResult GetGroupsBookedToRoom()
        {
            try
            {
                var result = _bookingService.GetBookingsForRoom();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("postGroupToRoom")]
        public IActionResult PostGroupToRoom([FromBody] GroupToRoomBookingDTO postGroupToRoomDTO)
        {
            try
            {
                _bookingService.PostGroupToRoomBooking(postGroupToRoomDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteGroupToRoomBooking")]
        public IActionResult DeleteGroupToRoomBooking(int Id)
        {
            try
            {
                _bookingService.DeleteGroupToRoomBooking(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteOldSingleRoomBookings")]
        public IActionResult DeleteOldSingleRoomBookings()
        {
            try
            {
                _bookingService.DeleteOldSingleRoomBookings();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("EditGroupToRoomBooking")]
        public IActionResult PutGroupToRoomBooking(int Id, [FromBody] GroupToRoomBookingDTO groupToRoomBooking)
        {
            try
            {
                _bookingService.EditGroupToRoomBooking(Id, groupToRoomBooking);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous] //Ändra denna till [Authorize(Roles = "Admin")] innan merge till main
        [HttpPost("Refresh")]
        public IActionResult Test()
        {
            try
            {
                _bookingService.RefreshBookings();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }
}
