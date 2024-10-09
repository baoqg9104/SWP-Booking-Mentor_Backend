using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO;
using SWP391_Mentor_Booking_System_Service.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO registerDTO)
        {
            if (_userService.IsEmailExist(registerDTO.Email))
            {
                return BadRequest("Email is already exist.");
            }

            var newUser = new User
            {
                FullName = registerDTO.FullName,
                Password = registerDTO.Password,
                Email = registerDTO.Email,
                Phone = null,
                Gender = null,
                DateOfBirth = null,
                RoleId = registerDTO.RoleId // Người dùng có thể chọn Student (1) hoặc Mentor (2)
            };

            _userService.RegisterUser(newUser);

            return Ok("User registered successfully.");
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDTO loginDto)
        {
            var user = _userService.Authenticate(loginDto.Email, loginDto.Password); // Đăng nhập bằng email

            if (user == null)
            {
                return Unauthorized("Thông tin đăng nhập không đúng hoặc mentor chưa được phê duyệt!"); // Cập nhật thông báo
            }

            var accessToken = GenerateToken(user);
            var refreshToken = _userService.GenerateRefreshToken();

            _userService.SaveRefreshToken(user.FullName, refreshToken);

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }


        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] string refreshToken)
        {
            var token = _userService.GetRefreshToken(refreshToken);

            if (token == null || token.ExpiryDate < DateTime.Now)
            {
                return Unauthorized("Refresh token không hợp lệ hoặc đã hết hạn.");
            }

            // Lấy lại thông tin user bằng UserName lưu trong refresh token
            var user = _userService.GetUserByUsername(token.UserName);
            if (user == null)
            {
                return Unauthorized("User không tồn tại.");
            }

            // Tạo access token mới
            var newAccessToken = GenerateToken(user);

            // Xóa refresh token cũ
            _userService.RevokeRefreshToken(refreshToken);

            // Tạo và lưu refresh token mới
            var newRefreshToken = _userService.GenerateRefreshToken();
            _userService.SaveRefreshToken(user.FullName, newRefreshToken);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }



        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim("FullName", user.FullName),
            new Claim("RoleId", user.RoleId.ToString()),
            new Claim("Email", user.Email), // Thêm email
            new Claim("Phone", user.Phone ?? ""), // Thêm số điện thoại
            new Claim("Gender", user.Gender ?? ""), // Thêm giới tính
            new Claim("DateOfBirth", user.DateOfBirth?.ToString("yyyy-MM-dd") ?? "") // Thêm ngày sinh
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
