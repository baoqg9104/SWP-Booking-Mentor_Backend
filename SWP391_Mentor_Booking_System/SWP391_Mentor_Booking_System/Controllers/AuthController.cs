using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO;
using SWP391_Mentor_Booking_System_Service;
using SWP391_Mentor_Booking_System_Service.Service;


namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDTO)
        {
            // Kiểm tra xem tên người dùng đã tồn tại chưa
            if (_userService.IsUsernameTaken(registerDTO.Username))
            {
                return BadRequest("Username is already taken.");
            }

            // Tạo người dùng mới mà không mã hóa mật khẩu
            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(), // Tạo ID ngẫu nhiên
                Username = registerDTO.Username,
                Password = registerDTO.Password, // Lưu mật khẩu trực tiếp
                Email = registerDTO.Email,
                Phone = registerDTO.Phone,
                RoleId = registerDTO.RoleId
            };

            // Đăng ký người dùng mới
            _userService.RegisterUser(newUser);

            return Ok("User registered successfully.");
        }
    }
}
