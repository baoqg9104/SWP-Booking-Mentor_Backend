using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.Group
{
    public class AddMemberDTO
    {
        public string GroupId { get; set; }
        public string LeaderId { get; set; }
        public List<string> Emails { get; set; }
    }
}
