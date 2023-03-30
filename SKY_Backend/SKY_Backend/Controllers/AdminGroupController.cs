using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Microsoft.AspNetCore.Authorization;
using Service.GroupService;
using Service.AdminGroupService;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGroupController : ControllerBase
    {
        private readonly IAdminGroupService _adminGroupService;

        public AdminGroupController(IAdminGroupService adminGroupService)
        {
            _adminGroupService = adminGroupService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateGroup/{groupId}")]
        public IActionResult UpdateGroup(int groupId, [FromBody] NewGroupInfoDTO newGroup)
        {
            try
            {
                _adminGroupService.UpdateGroup(groupId, newGroup);
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
                _adminGroupService.DeleteGroup(groupId);
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
                _adminGroupService.AddGroup(addGroupDTO);
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
