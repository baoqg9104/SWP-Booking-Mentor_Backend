using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.Semester;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SemesterController : ControllerBase
    {
        private readonly SemesterService _semesterService;

        public SemesterController(SemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        // Create
        [HttpPost("create")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateSemester([FromBody] CreateSemesterDTO createSemesterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _semesterService.CreateSemesterAsync(createSemesterDto);
            if (!result)
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok();
        }

        // Read by Id
        [HttpGet("get/{id}")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> GetSemesterById(string id)
        {
            var semester = await _semesterService.GetSemesterByIdAsync(id);
            if (semester == null)
                return NotFound();

            return Ok(semester);
        }

        // Read all
        [HttpGet("all")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> GetAllSemesters()
        {
            var semesters = await _semesterService.GetAllSemestersAsync();
            return Ok(semesters);
        }

        // Update
        [HttpPut("update")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateSemester([FromBody] UpdateSemesterDTO updateSemesterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _semesterService.UpdateSemesterAsync(updateSemesterDto);
            if (!result)
                return NotFound();

            return Ok();
        }

        // Delete
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteSemester(string id)
        {
            var result = await _semesterService.DeleteSemesterAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}
