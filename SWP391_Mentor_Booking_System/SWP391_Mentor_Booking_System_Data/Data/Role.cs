using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class Role
    {
        public int RoleId { get; set; }  // ID của Role
        public string Name { get; set; }  // Tên vai trò, ví dụ 'student', 'mentor'

        public ICollection<User> Users { get; set; }  // Một Role có thể có nhiều User
    }




}
