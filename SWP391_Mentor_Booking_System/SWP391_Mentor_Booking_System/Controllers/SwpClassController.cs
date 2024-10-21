using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.SwpClass;
using SWP391_Mentor_Booking_System_Service.Service;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SwpClassController : ControllerBase
    {
        private readonly SwpClassService _swpClassService;

        public SwpClassController(SwpClassService swpClassService)
        {
            _swpClassService = swpClassService;
        }

        // Create class
        [HttpPost("create")]
        public async Task<IActionResult> CreateClass([FromBody] CreateSwpClassDTO createSwpClassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _swpClassService.CreateClassAsync(createSwpClassDto);
            if (!result)
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok();
        }

        // Read class by Id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            var swpClass = await _swpClassService.GetClassByIdAsync(id);
            if (swpClass == null)
                return NotFound();

            return Ok(swpClass);
        }

        // Read all classes
        [HttpGet("all")]
        public async Task<IActionResult> GetAllClasses()
        {
            var classes = await _swpClassService.GetAllClassesAsync();
            return Ok(classes);
        }

        [HttpPut("update/{id}")]
        
        public async Task<IActionResult> UpdateClassById(int id, [FromBody] UpdateSwpClassDTO updateSwpClassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _swpClassService.UpdateClassByIdAsync(id, updateSwpClassDto);
            if (!result)
                return NotFound();

            return Ok();
        }


        // Delete class
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var result = await _swpClassService.DeleteClassAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }
}
