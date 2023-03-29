using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.BookingService;
using Service.DTO;
using System.Globalization;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingleBookingController : ControllerBase
    {
        private readonly ISingleBookingService _bookingService;

        public SingleBookingController(ISingleBookingService bookingService)
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
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }
}
