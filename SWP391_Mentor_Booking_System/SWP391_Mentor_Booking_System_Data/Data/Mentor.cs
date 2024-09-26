using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Mentor
    {
        public int MentorId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<MentorSkill> MentorSkills { get; set; }
        public ICollection<MentorSlot> MentorSlots { get; set; }
    }
}
