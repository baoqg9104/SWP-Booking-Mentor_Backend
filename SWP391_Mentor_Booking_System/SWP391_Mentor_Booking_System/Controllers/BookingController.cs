using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO.BookingSlot;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Create
        [HttpPost("create")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _bookingService.CreateBookingAsync(createBookingDto);

            if (!success)
                return BadRequest($"Error: {error}");

            return Ok();
        }

        // Update Booking Status
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateBookingStatus([FromBody] UpdateBookingStatusDTO updateBookingStatusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _bookingService.UpdateBookingStatusAsync(updateBookingStatusDto);
            if (!success)
                return NotFound();

            return Ok();
        }

        // Get Bookings by MentorSlotId
        [HttpGet("get-by-mentorslot/{mentorSlotId}")]
        public async Task<IActionResult> GetBookingsByMentorSlotId(int mentorSlotId)
        {
            var bookings = await _bookingService.GetBookingsByMentorSlotIdAsync(mentorSlotId);
            return Ok(bookings);
        }
    }

}
