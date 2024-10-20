using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.Group;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        // Create
        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDTO createGroupDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _groupService.CreateGroupAsync(createGroupDto);

            if (!success)
                return BadRequest($"Error: {error}");

            return Ok();
        }

        // Read by Id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetGroupById(string id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
                return NotFound();

            return Ok(group);
        }

        // Read all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        // Update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateGroup([FromBody] UpdateGroupDTO updateGroupDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _groupService.UpdateGroupAsync(updateGroupDto);

            if (!success)
                return BadRequest($"Error: {error}");

            return Ok();
        }

        // Delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteGroup(string id)
        {
            var result = await _groupService.DeleteGroupAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }

}
