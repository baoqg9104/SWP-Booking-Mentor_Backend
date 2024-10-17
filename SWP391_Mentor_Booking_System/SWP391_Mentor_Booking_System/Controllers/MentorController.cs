using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MentorController : ControllerBase
    {
        private readonly MentorService _mentorService;

        public MentorController(MentorService mentorService)
        {
            _mentorService = mentorService;
        }

        // Endpoint to change apply status
        [HttpPut("change-apply-status/{mentorId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> ChangeApplyStatus(string mentorId)
        {
            var result = await _mentorService.ChangeMentorApplyStatusAsync(mentorId);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMentors()
        {
            var mentors = await _mentorService.GetAllMentorsAsync();
            return Ok(mentors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMentorById(string id)
        {
            var mentor = await _mentorService.GetMentorByIdAsync(id);
            if(mentor == null)
            {
                return NotFound("Mentor not found.");
            }
            return Ok(mentor);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteMentor(string id)
        {
            try
            {
                var result = await _mentorService.DeleteMentorAsync(id);
                if (!result)
                {
                    return NotFound("Mentor not found.");
                }
                return Ok("Mentor deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
