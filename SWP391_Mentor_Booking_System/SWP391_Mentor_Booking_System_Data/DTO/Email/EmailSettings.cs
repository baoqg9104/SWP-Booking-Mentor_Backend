using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.Email
{
    public class EmailSettings
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
    }

}
