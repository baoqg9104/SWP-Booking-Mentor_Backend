using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.Topic
{
    public class UpdateTopicDTO
    {
        public int TopicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SemesterId { get; set; }
        public string Actors { get; set; }
        public bool Status { get; set; }
    }

}
