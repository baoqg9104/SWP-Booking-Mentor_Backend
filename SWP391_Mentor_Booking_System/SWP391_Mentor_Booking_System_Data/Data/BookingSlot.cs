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
        public int GroupId { get; set; }
        public int SlotId { get; set; }
        public bool Status { get; set; }

        public Group Group { get; set; }
        public MentorSlot MentorSlot { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; }
    }
}
