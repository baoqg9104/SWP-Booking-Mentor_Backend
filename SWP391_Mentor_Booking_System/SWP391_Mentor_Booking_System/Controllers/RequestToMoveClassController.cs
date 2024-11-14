using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.RequestToMoveClass;
using SWP391_Mentor_Booking_System_Data.DTO.Semester;
using SWP391_Mentor_Booking_System_Service.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestToMoveClassController : Controller
    {
        private readonly RequestToMoveClassService _requestToMoveClassService;

        public RequestToMoveClassController(RequestToMoveClassService requestToMoveClassService)
        {
            _requestToMoveClassService = requestToMoveClassService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRequestToMoveClass(CreateRequestToMoveClassDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _requestToMoveClassService.CreateRequestToMoveClassAsync(dto);

            if (!result.success)
                return BadRequest($"{result.error}");

            return Ok();
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetRequestToMoveClasses()
        {
            var result = await _requestToMoveClassService.GetRequestToMoveClassesAsync();

            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRequestToMoveClass(UpdateRequestToMoveClassDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _requestToMoveClassService.UpdateRequestToMoveClassAsync(dto);

            if (!result.success)
                return BadRequest($"{result.error}");

            return Ok();
        }

    }
}
