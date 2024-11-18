using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("all")]
        [Authorize(Policy = "MentorOnly")]

        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "StudentOrAdmin")]
        public async Task<IActionResult> GetStudentById(string id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }
            return Ok(student);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                var result = await _studentService.DeleteStudentAsync(id);
                if (!result)
                {
                    return NotFound("Student not found.");
                }
                return Ok("Student deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
