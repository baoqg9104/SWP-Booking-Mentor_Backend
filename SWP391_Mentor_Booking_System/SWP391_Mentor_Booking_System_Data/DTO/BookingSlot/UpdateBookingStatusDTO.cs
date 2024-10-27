using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.BookingSlot
{
    public class UpdateBookingStatusDTO
    {
        public int BookingId { get; set; }
        public int MentorSlotId { get; set; }
        public string Status { get; set; }
    }

}
