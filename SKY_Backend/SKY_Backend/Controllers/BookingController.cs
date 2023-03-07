using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;
using System.Globalization;

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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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
                throw new Exception(ex.StackTrace);
            }
        }

        //[FromBody] UpdateBookingsDTO[] updateBookingsDTO, 

        //[Authorize]
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



        //[Authorize]
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

        //[Authorize]
        [HttpPost("postGroupToRoom")]
        public IActionResult postGroupToRoom([FromBody]PostGroupToRoomDTO postGroupToRoomDTO)
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
