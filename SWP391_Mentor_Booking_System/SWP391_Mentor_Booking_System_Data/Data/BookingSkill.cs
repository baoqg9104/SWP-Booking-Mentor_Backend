using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class BookingSkill
    {
        public int BookingSkillId { get; set; }
        public int BookingSlotId { get; set; }
        public BookingSlot BookingSlot { get; set; }
        public int MentorSkillId { get; set; }
        public MentorSkill MentorSkill { get; set; }
    }
}
