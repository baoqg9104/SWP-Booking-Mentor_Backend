using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.FeedBack
{
    public class FeedbackDTO
    {
        public int BookingId { get; set; }
        public int? GroupRating { get; set; }
        public int? MentorRating { get; set; }
        public string? GroupFeedback { get; set; }
        public string? MentorFeedback { get; set; }
    }
}
