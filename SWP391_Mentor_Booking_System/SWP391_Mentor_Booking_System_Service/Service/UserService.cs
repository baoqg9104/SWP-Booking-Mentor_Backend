using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly RefreshTokenRepository _refreshTokenRepository; 
        private readonly IConfiguration _configuration;

        public UserService(UserRepository userRepository, RefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository; 
            _configuration = configuration;
        }


        public bool IsUsernameTaken(string username)
        {
            return _userRepository.UserExists(username);
        }

        public void RegisterUser(User user)
        {
            user.RoleId = 1;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _userRepository.AddUser(user);
            var student = new Student
            {
                StudentId = GenerateStudentId(),
                StudentName = user.UserName,
                GroupId = null,
                User = user
            };
            _userRepository.AddStudent(student);
        }

        private string GenerateStudentId()
        {
            int count = _userRepository.GetTotalStudents();
            return $"SE{count + 1:000}";
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null; // Đăng nhập thất bại
            }
            return user; // Đăng nhập thành công
        }
        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }
        // Tạo refresh token
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        // Lưu refresh token vào database
        public void SaveRefreshToken(string userName, string refreshToken)
        {
            var newRefreshToken = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.Now.AddDays(7), // Refresh token hết hạn sau 7 ngày
                UserName = userName
            };

            _refreshTokenRepository.AddRefreshToken(newRefreshToken);
        }

        // Kiểm tra tính hợp lệ của refresh token
        public RefreshToken GetRefreshToken(string token)
        {
            return _refreshTokenRepository.GetByToken(token);
        }

        // Xóa refresh token sau khi sử dụng
        public void RevokeRefreshToken(string token)
        {
            _refreshTokenRepository.RemoveToken(token);
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
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"])), // Thời gian hết hạn
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
