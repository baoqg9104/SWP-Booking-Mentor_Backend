using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class SwpClass
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string MentorId { get; set; }
        public Mentor Mentor { get; set; }

        public string SemesterId { get; set; }
        public Semester Semester { get; set; }

        public bool Status { get; set; }

        // Relationship with Group
        public ICollection<Group> Groups { get; set; }

    }


}
