using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace SKY_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        [HttpGet("get-groups")]
        public IActionResult GetGroups()
        {
            IGroupService groupService = new GroupService();

            try
            {
                var result = groupService.GetGroups();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-group")]
        public IActionResult GetGroup(int id)
        {
            IGroupService groupService = new GroupService();

            try
            {
                var result = groupService.GetGroup(id);

                if(result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
