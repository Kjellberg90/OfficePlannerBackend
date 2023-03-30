using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.SingleBookingService;
using System.Globalization;
//using System.Web.Http;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SingleBookingController : ControllerBase
    {
        private readonly ISingleBookingService _singleBookingService;

        public SingleBookingController(ISingleBookingService singlBookingService)
        {
            _singleBookingService = singlBookingService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("bookings")]
        public IActionResult GetBookings()
        {
            try
            {
                var result = _singleBookingService.GetBookings();
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
                var result = _singleBookingService.GetSingleBookingsForDate(date, roomId);
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
                _singleBookingService.PostSingleBooking(singleBooking);
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
                _singleBookingService.DeleteSingleBooking(deleteSingleBooking);
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

        [AllowAnonymous]
        [HttpPost("post")]
        public IActionResult PostBookings()
        {
            _singleBookingService.PostBookings();
            return Ok();
        }
    }
}
