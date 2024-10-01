using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Relationships
        public ICollection<MentorSkill> MentorSkills { get; set; }
    }



}
