using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;
using System.Globalization;
//using System.Web.Http;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("bookings")]
        public IActionResult GetBookings()
        {
            try
            {
                var result = _bookingService.GetBookings();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetSingleBookings")]
        public IActionResult GetSingleBookings(string date, int roomId)
        {
            try
            {
                var result = _bookingService.GetSingleBookingsForDate(date, roomId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("SingleBooking")]
        public IActionResult PostSingleBooking([FromBody] SingleBookingDTO singleBooking)
        {
            try
            {
                _bookingService.PostSingleBooking(singleBooking);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("DeleteSingleBooking")]
        public IActionResult DeleteBookings([FromBody]DeleteSingleBookingDTO deleteSingleBooking)
        {
            try
            {
                _bookingService.DeleteSingleBooking(deleteSingleBooking);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Incorrect password")
                {
                    return StatusCode(401, ex.Message);
                }

                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
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

        [AllowAnonymous]
        [HttpPost("post")]
        public IActionResult PostBookings()
        {
            _bookingService.PostBookings();
            return Ok();
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
        public IActionResult PostGroupToRoom([FromBody]GroupToRoomBookingDTO postGroupToRoomDTO)
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
        public IActionResult PutGroupToRoomBooking(int Id, [FromBody]GroupToRoomBookingDTO groupToRoomBooking)
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

        [AllowAnonymous]
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
