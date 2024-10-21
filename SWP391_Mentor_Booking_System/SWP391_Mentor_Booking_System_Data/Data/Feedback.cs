using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Feedback
    {
        public int FeedbackId { get; set; }

        public int BookingId { get; set; }
        public int GroupRating { get; set; }
        public int MentorRating { get; set; }
        public string? GroupFeedback { get; set; }
        public string? MentorFeedback { get; set; }
        public BookingSlot BookingSlot { get; set; }

    }
}
