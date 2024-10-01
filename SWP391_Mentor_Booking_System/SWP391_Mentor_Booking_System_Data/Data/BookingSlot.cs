using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class BookingSlot
    {
        public int Id { get; set; }

        // Foreign Key
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int MentorSlotId { get; set; }
        public MentorSlot MentorSlot { get; set; }

        public bool Status { get; set; }
    }


}
