using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.Repositories;


namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Kiểm tra username có tồn tại hay chưa
        public bool IsUsernameTaken(string username)
        {
            return _userRepository.UserExists(username);
        }

        // Đăng ký người dùng mới
        public void RegisterUser(User user)
        {
            // Gán RoleId mặc định là 1
            user.RoleId = 1;

            // Thêm người dùng vào cơ sở dữ liệu
            _userRepository.AddUser(user);
        }
        // Đăng nhập người dùng
        public User Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null || user.Password != password)
            {
                return null; // Đăng nhập thất bại
            }
            return user; // Đăng nhập thành công
        }
        
    }
}
