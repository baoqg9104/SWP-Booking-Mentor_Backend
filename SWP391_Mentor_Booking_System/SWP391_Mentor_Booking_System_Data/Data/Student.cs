using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Student
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public Group Group { get; set; }
    }


}
