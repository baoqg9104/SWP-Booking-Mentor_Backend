using SWP391_Mentor_Booking_System_Data.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Data.Repositories
{
    public class UserRepository
    {
        private readonly SWP391_Mentor_Booking_System_DBContext _context; 

        public UserRepository(SWP391_Mentor_Booking_System_DBContext context)
        {
            _context = context;
        }

        // Kiểm tra username có tồn tại hay không
        public bool UserExists(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        // Thêm người dùng mới vào database
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges(); // Lưu thay đổi vào database
        }
        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }
        
    }
}
