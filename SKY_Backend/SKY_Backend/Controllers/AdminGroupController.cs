using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTO;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public AdminGroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
    }
}
