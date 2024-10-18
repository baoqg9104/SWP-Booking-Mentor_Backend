using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MentorSlotController : ControllerBase
    {
        private readonly MentorSlotService _mentorSlotService;

        public MentorSlotController(MentorSlotService mentorSlotService)
        {
            _mentorSlotService = mentorSlotService;
        }

        // Create
        [HttpPost("create")]
        public async Task<IActionResult> CreateMentorSlot([FromBody] CreateMentorSlotDTO createMentorSlotDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _mentorSlotService.CreateMentorSlotAsync(createMentorSlotDto);

            if (!success)
                return BadRequest($"Error: {error}");

            return Ok();
        }

        // Read by MentorSlotId
        [HttpGet("get-by-id/{id}")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> GetMentorSlotById(int id)
        {
            var mentorSlot = await _mentorSlotService.GetMentorSlotByIdAsync(id);
            if (mentorSlot == null)
                return NotFound();

            return Ok(mentorSlot);
        }

        // Read all
        [HttpGet("all")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> GetAllMentorSlots()
        {
            var mentorSlots = await _mentorSlotService.GetAllMentorSlotsAsync();
            return Ok(mentorSlots);
        }

        // Read by MentorId
        [HttpGet("get-by-mentor-id/{mentorId}")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> GetMentorSlotsByMentorId(string mentorId)
        {
            var mentorSlots = await _mentorSlotService.GetMentorSlotsByMentorIdAsync(mentorId);
            return Ok(mentorSlots);
        }

        // Update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateMentorSlot([FromBody] UpdateMentorSlotDTO updateMentorSlotDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _mentorSlotService.UpdateMentorSlotAsync(updateMentorSlotDto);

            if (!success)
                return BadRequest($"Error: {error}");

            return Ok();
        }

        // Delete
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "MentorOnly")]
        public async Task<IActionResult> DeleteMentorSlot(int id)
        {
            var result = await _mentorSlotService.DeleteMentorSlotAsync(id);
            if (!result)
                return NotFound();

            return Ok("This mentor slot is deleted");
        }
    }

}
