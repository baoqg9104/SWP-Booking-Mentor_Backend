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

            var result = await _mentorSlotService.CreateMentorSlotAsync(createMentorSlotDto);
            if (!result)
                return BadRequest("Mentor ID does not exist or other validation failed.");

            return Ok("Mentor Slot creation complete");
        }

        // Read by MentorSlotId
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetMentorSlotById(int id)
        {
            var mentorSlot = await _mentorSlotService.GetMentorSlotByIdAsync(id);
            if (mentorSlot == null)
                return NotFound();

            return Ok(mentorSlot);
        }

        // Read all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMentorSlots()
        {
            var mentorSlots = await _mentorSlotService.GetAllMentorSlotsAsync();
            return Ok(mentorSlots);
        }

        // Read by MentorId
        [HttpGet("get-by-mentor-id/{mentorId}")]
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

            var result = await _mentorSlotService.UpdateMentorSlotAsync(updateMentorSlotDto);
            if (!result)
                return NotFound();

            return Ok("Mentor Slot Updated Successfully");
        }

        // Delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMentorSlot(int id)
        {
            var result = await _mentorSlotService.DeleteMentorSlotAsync(id);
            if (!result)
                return NotFound();

            return Ok("This mentor slot is deleted");
        }
    }

}
