using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.Group
{
    public class CreateGroupDTO
    {
        public string LeaderId { get; set; }
        public string Name { get; set; }
        public int TopicId { get; set; }
        public int SwpClassId { get; set; }
        public List<string> MemberEmails { get; set; }
    }

}
