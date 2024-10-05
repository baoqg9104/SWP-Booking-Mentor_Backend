using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Student
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }  // Tên sinh viên sẽ liên kết với username
        public int? GroupId { get; set; }

        public User User { get; set; }  // Liên kết với Username bên User
        public Group Group { get; set; }
    }





}
