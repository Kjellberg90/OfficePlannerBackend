using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.AdminRoomBookingService;
using Service.BookingService;
using Service.DTO;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomBookingController : ControllerBase
    {
        private readonly IAdminRoomBookingService _adminBookingService;

        public AdminRoomBookingController(IAdminRoomBookingService adminBookingService)
        {
            _adminBookingService = adminBookingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateBookings/{date}")]
        public IActionResult UpdateBookings([FromBody] UpdateBookingsDTO[] updateBookingsDTO, string date)
        {
            try
            {
                _adminBookingService.UpdateBookings(updateBookingsDTO, date);
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
                var result = _adminBookingService.GetBookingsForRoom();
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
                _adminBookingService.PostGroupToRoomBooking(postGroupToRoomDTO);
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
                _adminBookingService.DeleteGroupToRoomBooking(Id);
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
                _adminBookingService.DeleteOldSingleRoomBookings();
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
                _adminBookingService.EditGroupToRoomBooking(Id, groupToRoomBooking);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Refresh")]
        public IActionResult Test()
        {
            try
            {
                _adminBookingService.RefreshBookings();
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
