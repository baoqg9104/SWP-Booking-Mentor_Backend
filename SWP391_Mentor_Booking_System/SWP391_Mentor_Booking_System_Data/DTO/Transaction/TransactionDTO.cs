using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.Transaction
{
    public class TransactionDTO
    {
        public int BookingId { get; set; }
        public string Type { get; set; }
        public int Point { get; set; }
        public DateTime DateTime { get; set; }
        public string GroupName { get; set; }
        public string SwpClassName { get; set; }
        public string MentorName { get; set; }
    }
}
