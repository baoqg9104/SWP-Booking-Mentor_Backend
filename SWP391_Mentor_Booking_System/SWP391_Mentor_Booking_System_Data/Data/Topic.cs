using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SemesterId { get; set; }
        public Semester Semester { get; set; }
        public bool Status { get; set; }

        // Relationships
        public ICollection<Group> Groups { get; set; }
    }

}
