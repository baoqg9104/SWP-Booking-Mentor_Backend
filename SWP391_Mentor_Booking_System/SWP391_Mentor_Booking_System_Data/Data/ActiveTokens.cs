using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class ActiveToken
    {
        public string TokenId { get; set; } // Khóa chính
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

}
