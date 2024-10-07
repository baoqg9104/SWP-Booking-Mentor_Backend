using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Semester
    {
        public string SemesterId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }

        // Relationships
        public ICollection<SwpClass> SwpClasses { get; set; }
        public ICollection<Topic> Topics { get; set; }
    }

}
