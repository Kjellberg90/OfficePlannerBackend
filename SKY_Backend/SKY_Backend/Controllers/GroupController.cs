using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.GroupService;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetGroups")]
        public IActionResult GetGroups()
        {
            try
            {
                var result = _groupService.GetGroups();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GroupInfo/{date}&{groupId}")]
        public IActionResult GroupInfo(string date, int groupId)
        {
            try
            {
                var result = _groupService.GetGroupInfo(date, groupId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetWeeklyGroupSchedule")]
        public IActionResult GetWeeklyScheduleForGroup(string date, int groupId)
        {
            try
            {
                var result =  _groupService.GetWeeklysSchedule(date, groupId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("GetCurrentWeekAndDay")]
        public IActionResult GetCurrentWeekAndDay(string date)
        {
            try
            {
                var result = _groupService.GetCurrentWeekAndDay(date);
                return Ok(result);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }
    }
}
