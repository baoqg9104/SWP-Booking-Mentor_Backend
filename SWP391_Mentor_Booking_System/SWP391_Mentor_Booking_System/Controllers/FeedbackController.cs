using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.FeedBack;
using SWP391_Mentor_Booking_System_Service.Service;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("submit")]
        [Authorize]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackRequestDTO feedbackDto)
        {
            var currentUserId = User.FindFirst("id")?.Value;
            // Lấy userId từ token
            var isFromMentor = feedbackDto.IsFromMentor; // Kiểm tra xem phản hồi đến từ Mentor hay Student

            // Nếu currentUserId null, trả về Unauthorized
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("User không xác thực.");
            }

            try
            {
                await _feedbackService.SubmitFeedback(feedbackDto, currentUserId, isFromMentor);
                return Ok("Feedback submitted successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message); // Trả về Forbidden nếu không có quyền
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Trả về BadRequest nếu có lỗi khác
            }
        }

        [HttpGet("{bookingId}")]
        [Authorize]
        public async Task<IActionResult> GetFeedbacks(int bookingId)
        {
            var feedbacks = await _feedbackService.GetFeedbacksForBooking(bookingId);
            return Ok(feedbacks);
        }
    }
}
