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
            if (_userService.IsUsernameTaken(registerDTO.UserName))
            {
                return BadRequest("Username is already taken.");
            }

            var newUser = new User
            {
                UserName = registerDTO.UserName,
                Password = registerDTO.Password,
                Email = registerDTO.Email,
                Phone = registerDTO.Phone,
                RoleId = 1 // Mặc định là student
            };

            // Đăng ký User và Student
            _userService.RegisterUser(newUser);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDTO loginDto)
        {
            var user = _userService.Authenticate(loginDto.Username, loginDto.Password);

            if (user == null)
            {
                return Unauthorized("Thông tin đăng nhập không đúng!");
            }

            var accessToken = GenerateToken(user);
            var refreshToken = _userService.GenerateRefreshToken();

            _userService.SaveRefreshToken(user.UserName, refreshToken);

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
            _userService.SaveRefreshToken(user.UserName, newRefreshToken);

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
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
