using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class SwpClass
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public int MentorId { get; set; }
        public int SemesterId { get; set; }

        public Mentor Mentor { get; set; }
        public Semester Semester { get; set; }
        public ICollection<Group> Groups { get; set; }
    }

}
