using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

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

        [AllowAnonymous]
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

        [Authorize]
        [HttpPut("UpdateGroup/{groupId}")]
        public IActionResult UpdateGroup(int groupId, [FromBody] NewGroupInfoDTO newGroup)
        {
            try
            {
                _groupService.UpdateGroup(groupId, newGroup);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("DeleteGroup/{groupId}")]
        public IActionResult DeleteGroup(int groupId)
        {
            try
            {
                _groupService.DeleteGroup(groupId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace, ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //[AllowAnonymous]
        //[HttpPost("RefreshData")]
        //public IActionResult RefreshData()
        //{
        //    try
        //    {
        //        _groupService.Refresh();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        [Authorize]
        [HttpPost("AddGroup")]
        public IActionResult AddGroup([FromBody] AddGroupDTO addGroupDTO)
        {
            try
            {
                _groupService.AddGroup(addGroupDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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
