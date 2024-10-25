using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class BookingSlot
    {
        public int BookingId { get; set; }

        // Foreign Key
        public string? GroupId { get; set; }
        public Group Group { get; set; }

        public int MentorSlotId { get; set; }
        public MentorSlot MentorSlot { get; set; }

        public int MentorSkillId { get; set; }
        public MentorSkill MentorSkill { get; set; }

        public DateTime BookingTime { get; set; }

        public string Status { get; set; }
        public Feedback Feedback { get; set; }
    }


}
