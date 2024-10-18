using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.SwpClass;
using SWP391_Mentor_Booking_System_Service.Service;

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

        // Create
        [HttpPost("create")]
        public async Task<IActionResult> CreateSwpClass([FromBody] CreateSwpClassDTO createSwpClassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _swpClassService.CreateSwpClassAsync(createSwpClassDto);
            if (!result)
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok();
        }

        // Read by Id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetSwpClassById(int id)
        {
            var swpClass = await _swpClassService.GetSwpClassByIdAsync(id);
            if (swpClass == null)
                return NotFound();

            return Ok(swpClass);
        }

        // Read all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSwpClasses()
        {
            var swpClasses = await _swpClassService.GetAllSwpClassesAsync();
            return Ok(swpClasses);
        }

        // Update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateSwpClass([FromBody] UpdateSwpClassDTO updateSwpClassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _swpClassService.UpdateSwpClassAsync(updateSwpClassDto);
            if (!result)
                return NotFound();

            return Ok();
        }

        // Delete
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSwpClass(int id)
        {
            var result = await _swpClassService.DeleteSwpClassAsync(id);
            if (!result)
                return NotFound();

            return Ok();
        }
    }

}
