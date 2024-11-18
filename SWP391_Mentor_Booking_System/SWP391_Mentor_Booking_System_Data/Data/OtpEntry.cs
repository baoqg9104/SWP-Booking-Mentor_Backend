using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class OtpEntry
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public DateTime OtpExpiry { get; set; }
    }
}
