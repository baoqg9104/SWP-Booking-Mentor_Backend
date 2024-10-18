using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.MentorSlot
{
    public class MentorSlotDTO
    {
        public int MentorSlotId { get; set; }
        public string MentorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int BookingPoint { get; set; }
        public bool isOnline { get; set; }
        public string? room { get; set; }
    }
}
