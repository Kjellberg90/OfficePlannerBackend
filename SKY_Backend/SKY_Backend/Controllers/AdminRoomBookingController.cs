using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Microsoft.AspNetCore.Authorization;
using Service;
using Service.AdminRoomBookingService;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomBookingController : ControllerBase
    {

        private readonly IAdminRoomBookingService _adminRoomBookingService;

        public AdminRoomBookingController(IAdminRoomBookingService adminRoomBookingService)
        {
            _adminRoomBookingService = adminRoomBookingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateBookings/{weekNr}")]
        public IActionResult UpdateBookings([FromBody] UpdateBookingsDTO[] updateBookingsDTO, int weekNr)
        {
            try
            {
                _adminRoomBookingService.UpdateBookings(updateBookingsDTO, weekNr);
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
                var result = _adminRoomBookingService.GetBookingsForRoom();
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
                _adminRoomBookingService.PostGroupToRoomBooking(postGroupToRoomDTO);
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
                _adminRoomBookingService.DeleteGroupToRoomBooking(Id);
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
                _adminRoomBookingService.DeleteOldSingleRoomBookings();
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
                _adminRoomBookingService.EditGroupToRoomBooking(Id, groupToRoomBooking);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Refresh")]
        public IActionResult Test()
        {
            try
            {
                _adminRoomBookingService.RefreshBookings();
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
