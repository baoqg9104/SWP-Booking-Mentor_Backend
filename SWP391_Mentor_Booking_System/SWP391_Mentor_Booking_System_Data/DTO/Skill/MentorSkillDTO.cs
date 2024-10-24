using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.DTO.Skill
{
    public class MentorSkillDTO
    {
        public int MentorSkillId { get; set; }
        public string MentorId { get; set; }
        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public int Level { get; set; }

    }
}
