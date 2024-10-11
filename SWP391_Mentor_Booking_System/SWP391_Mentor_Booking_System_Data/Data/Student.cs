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
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? GroupId { get; set; }
        

       
    
        
        public Group Group { get; set; }
    }





}
