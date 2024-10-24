using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.Skill;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly SkillService _skillService;

        public SkillsController(SkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllSkills()
        {
            var skills = await _skillService.GetAllSkillsAsync();
            return Ok(skills);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkillById(int id)
        {
            var skill = await _skillService.GetSkillByIdAsync(id);
            if (skill == null)
                return NotFound();

            return Ok(skill);
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddSkill([FromBody] CreateSkillDTO createskillDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _skillService.AddSkillAsync(createskillDto);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSkill( [FromBody] UpdateSkillDTO updateskillDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _skillService.UpdateSkillByIdAsync(updateskillDto);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var result = await _skillService.DeleteSkillAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }

        [HttpPost("addskill")]
        public async Task<IActionResult> AddMentorSkill([FromBody] AddMentorSkillDTO dto)
        {
            try
            {
                await _skillService.AddMentorSkillAsync(dto);
                return Ok("Thêm kỹ năng cho mentor thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("mentorskill/{mentorId}")]
        public async Task<IActionResult> GetMentorSkillsByMentorId(string mentorId)
        {
            try
            {
                var mentorSkills = await _skillService.GetMentorSkillsByMentorIdAsync(mentorId);
                return Ok(mentorSkills);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
