using DAL.Models;
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

        [HttpGet("group")]
        public IActionResult GetGroup(int groupId)
        {
            try
            {
                var result = _groupService.GetGroup(groupId);

                if (result == null)
                {
                    return NotFound();
                } 

                return Ok(result);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("groups")]
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
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("group-info")]
        public IActionResult GetGroupInfo(int groupId)
        {
            try
            {
                var result = _groupService.GetGroupInfo(groupId);

                if (result == null)
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

        [HttpPost("group")]
        public IActionResult PostGroupToFile(PostGroupDTO data)
        {
            try
            {
                _groupService.PostGroup(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("group")]
        public IActionResult DeleteGroup(int groupId)
        {
            try
            {
                _groupService.DeleteGroup(groupId);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-group")]
        public IActionResult UpdateGroup(Group group)
        {
            try
            {
                _groupService.UpdateGroup(group);
                return Ok();
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
