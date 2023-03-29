using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.AdminGroupService;
using Service.DTO;
using Service.GroupService;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGroupController : ControllerBase
    {
        private readonly IAdminGroupService _admingroupService;

        public AdminGroupController(IAdminGroupService admingroupService)
        {
            _admingroupService = admingroupService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateGroup/{groupId}")]
        public IActionResult UpdateGroup(int groupId, [FromBody] NewGroupInfoDTO newGroup)
        {
            try
            {
                _admingroupService.UpdateGroup(groupId, newGroup);
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
                _admingroupService.DeleteGroup(groupId);
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
                _admingroupService.AddGroup(addGroupDTO);
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
