using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO.FeedBack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class FeedbackService
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context;

        public FeedbackService(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        public async Task SubmitFeedback(FeedbackRequestDTO feedbackDto, string currentUserId, bool isFromMentor)
        {
            var bookingSlot = await _context.BookingSlots
                                             .Include(bs => bs.Group)
                                             .Include(bs => bs.MentorSlot)
                                             .Include(bs => bs.Feedback)  // Include feedback for update
                                             .FirstOrDefaultAsync(bs => bs.BookingId == feedbackDto.BookingId);

            if (bookingSlot == null || bookingSlot.Status != "Completed")
            {
                throw new Exception("Booking slot not found or is not completed.");
            }

            if (bookingSlot.Feedback == null)
            {
                bookingSlot.Feedback = new Feedback { BookingId = feedbackDto.BookingId };
                _context.Feedbacks.Add(bookingSlot.Feedback);
            }

            if (isFromMentor)
            {
                // Phản hồi từ Mentor
                var mentor = await _context.Mentors.FindAsync(currentUserId);
                if (mentor == null)
                    throw new UnauthorizedAccessException("Only the mentor can submit feedback for the group.");

                bookingSlot.Feedback.MentorRating = feedbackDto.Rating;
                bookingSlot.Feedback.MentorFeedback = feedbackDto.FeedbackText;
            }
            else
            {
                // Phản hồi từ Leader của Group
                if (bookingSlot.Group.LeaderId != currentUserId)
                    throw new UnauthorizedAccessException("Only the leader of the group can submit feedback for the mentor.");

                bookingSlot.Feedback.GroupRating = feedbackDto.Rating;
                bookingSlot.Feedback.GroupFeedback = feedbackDto.FeedbackText;
            }

            await _context.SaveChangesAsync();
        }


        public async Task<List<FeedbackResponseDTO>> GetFeedbacksForBooking(int bookingId)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.BookingSlot)
                    .ThenInclude(bs => bs.Group)
                .Where(f => f.BookingId == bookingId)
                .ToListAsync();

            return feedbacks.Select(f => new FeedbackResponseDTO
            {
                BookingId = f.BookingId,
                // Lấy Rating từ cả Mentor và Group, nếu có
                Rating = f.MentorRating ?? f.GroupRating ?? 0, 
                FeedbackText = f.MentorFeedback ?? f.GroupFeedback ?? "No feedback provided", 
                GroupName = f.BookingSlot.Group.Name,
                ClassName = f.BookingSlot.Group.SwpClass.Name 
            }).ToList();
        }

    }


}
