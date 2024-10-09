using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Mentor
    {
        public string MentorId { get; set; }
        public string MentorName { get; set; }  // Tên mentor sẽ liên kết với username
        public int? PointsReceived { get; set; }
        public int? NumOfSlot { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool ApplyStatus { get; set; } = false;

        public User User { get; set; }  // Liên kết với Username bên User
        public ICollection<MentorSkill> MentorSkills { get; set; }
        public ICollection<MentorSlot> MentorSlots { get; set; }
        public ICollection<SwpClass> SwpClasses { get; set; }
    }






}
