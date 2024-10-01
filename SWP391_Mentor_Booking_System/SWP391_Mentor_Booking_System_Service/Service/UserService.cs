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
            _userRepository.AddUser(user);
        }
    }
}
