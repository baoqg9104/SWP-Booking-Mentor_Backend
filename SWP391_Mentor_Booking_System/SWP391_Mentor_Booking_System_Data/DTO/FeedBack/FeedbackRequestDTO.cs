using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.FeedBack
{
    public class FeedbackRequestDTO
    {
        public int BookingId { get; set; }
        public int Rating { get; set; }
        public string FeedbackText { get; set; }
        public bool IsFromMentor { get; set; }  // true nếu là từ Mentor, false nếu từ Leader của Group
    }


}
