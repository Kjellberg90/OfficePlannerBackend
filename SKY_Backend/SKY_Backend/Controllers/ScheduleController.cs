using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.ScheduleService.ScheduleService;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet("schedule-weeks/{scheduleId}")]
        public IActionResult GetWeeksTotal(int scheduleId)
        {
            try
            {
                var result = _scheduleService.GetWeeksTotal(scheduleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetSchedules")]
        public IActionResult GetSchedules()
        {
            try
            {
                var result = _scheduleService.GetSchedules();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateBookings/{weekNr}")]
        public IActionResult UpdateBookings([FromBody] UpdateBookingsDTO[] updateBookingsDTO, int weekNr)
        {
            try
            {
                _scheduleService.UpdateBookings(updateBookingsDTO, weekNr);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
