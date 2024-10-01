using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class MentorSkill
    {
        public int Id { get; set; }

        // Foreign Keys
        public string MentorId { get; set; }
        public Mentor Mentor { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }

        public int Level { get; set; }
    }


}
