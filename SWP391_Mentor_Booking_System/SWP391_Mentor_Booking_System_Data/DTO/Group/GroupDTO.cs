using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.Group
{
    public class GroupDTO
    {
        public string GroupId { get; set; }
        public string Name { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public int SwpClassId { get; set; }
        public string SwpClassName { get; set; }
        public int WalletPoint { get; set; }
        public int Progress { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
