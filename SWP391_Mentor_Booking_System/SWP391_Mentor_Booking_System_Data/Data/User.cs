using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Data
{
    public class User
    {
        
        public string UserName { get; set; }  // Tên đăng nhập của người dùng
        public string Password { get; set; }  // Mật khẩu đã được mã hóa
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }  // Khóa ngoại liên kết với Role

        public Role Role { get; set; }  // Mỗi User có một vai trò
        public Mentor Mentor { get; set; }  // Một User có thể là Mentor
        public Student Student { get; set; }  // Một User có thể là Student
    }






}
