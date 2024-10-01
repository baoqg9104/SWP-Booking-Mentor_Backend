using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Student
    {
        public string Id { get; set; }  // ID của Student
        public int GroupId { get; set; }  // Khóa ngoại liên kết với nhóm của sinh viên
        public string UserId { get; set; }  // Khóa ngoại liên kết với User

        public User User { get; set; }  // Student liên kết với một User
        public Group Group { get; set; }  // Student liên kết với một nhóm (Group)
    }





}
