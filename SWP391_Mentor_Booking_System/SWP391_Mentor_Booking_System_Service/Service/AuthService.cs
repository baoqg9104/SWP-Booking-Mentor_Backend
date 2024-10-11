using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO;
using SWP391_Mentor_Booking_System_Data.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly RefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;

        public AuthService(AuthRepository authRepository, IConfiguration configuration, RefreshTokenRepository refreshTokenRepository)
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task RegisterAsync(RegisterDTO registerDto)
        {
            if (_authRepository.EmailExists(registerDto.Email))
            {
                throw new Exception("Email đã tồn tại.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            if (registerDto.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                var student = new Student
                {
                    StudentId = GenerateStudentId(),
                    StudentName = registerDto.FullName,
                    Email = registerDto.Email,
                    Password = hashedPassword,
                    Phone = null,
                    Gender = null,
                    DateOfBirth = null,
                    GroupId = null,
                };
                _authRepository.AddStudent(student);
            }
            else if (registerDto.Role.Equals("Mentor", StringComparison.OrdinalIgnoreCase))
            {
                var mentor = new Mentor
                {
                    MentorId = GenerateMentorId(),
                    MentorName = registerDto.FullName,
                    Email = registerDto.Email,
                    Password = hashedPassword,
                    Phone = null,
                    Gender = null,
                    DateOfBirth = null,
                    PointsReceived = null,
                    NumOfSlot = null,
                    RegistrationDate = DateTime.Now,
                  
                    ApplyStatus = false // Đặt ApplyStatus là false
                    // Các trường khác để null
                };
                _authRepository.AddMentor(mentor);
            }

            await _authRepository.SaveChangesAsync(); // Đảm bảo gọi phương thức SaveChangesAsync từ repository
        }

        // Phương thức đăng nhập cho Student
        public async Task<(string accessToken, string refreshToken)> LoginStudentAsync(LoginDTO loginDto)
        {
            var student = await _authRepository.GetStudentByEmailAsync(loginDto.Email);
            if (student == null)
            {
                throw new Exception("Email không tồn tại.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, student.Password))
            {
                throw new Exception("Mật khẩu không chính xác.");
            }

            // Tạo token mới
            var accessToken = GenerateToken(student);
            var refreshToken = GenerateRefreshToken(student.StudentId);

            // Lưu refresh token vào cơ sở dữ liệu
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = student.StudentId,
                ExpiryDate = DateTime.UtcNow.AddDays(7) // Thời gian hết hạn 7 ngày
            };
            _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        // Phương thức đăng nhập cho Mentor
        public async Task<(string accessToken, string refreshToken)> LoginMentorAsync(LoginDTO loginDto)
        {
            var mentor = await _authRepository.GetMentorByEmailAsync(loginDto.Email);

            if (mentor == null)
            {
                throw new Exception("Email không tồn tại.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, mentor.Password))
            {
                throw new Exception("Mật khẩu không chính xác.");
            }

            if (!mentor.ApplyStatus)
            {
                throw new Exception("Đợi admin approved.");
            }

            var accessToken = GenerateToken(mentor);
            var refreshToken = GenerateRefreshToken(mentor.MentorId);

            // Lưu refresh token vào cơ sở dữ liệu
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = mentor.MentorId,
                ExpiryDate = DateTime.UtcNow.AddDays(7) // Hết hạn sau 7 ngày
            };
            _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        // Phương thức đăng nhập cho Admin
        public async Task<(string accessToken, string refreshToken)> LoginAdminAsync(LoginDTO loginDto)
        {
            var admin = await _authRepository.GetAdminByEmailAsync(loginDto.Email);

            if (admin == null)
            {
                throw new Exception("Email không tồn tại.");
            }

            // So sánh mật khẩu trực tiếp (không dùng mã hóa)
            if (loginDto.Password != admin.Password)
            {
                throw new UnauthorizedAccessException("Mật khẩu không chính xác.");
            }

            // Tạo token cho admin
            var accessToken = GenerateToken(admin);
            var refreshToken = GenerateRefreshToken(admin.AdminId);

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = admin.AdminId,
                ExpiryDate = DateTime.UtcNow.AddDays(7) // Hết hạn sau 7 ngày
            };
            _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (accessToken, refreshToken);
        }
        private string GenerateMentorId()
        {
            int count = _authRepository.GetTotalMentors();
            return $"ME{count + 1:000}";
        }

        private string GenerateStudentId()
        {
            int count = _authRepository.GetTotalStudents();
            return $"SE{count + 1:000}";
        }

        // Tạo token JWT (phương thức mẫu, bạn thay thế với logic của mình)
       


        private string GenerateToken(Student student)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, student.StudentId),
        new Claim("accountType", "Student"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("fullName", student.StudentName),
        new Claim("email", student.Email),
        new Claim("phone", student.Phone ?? string.Empty),
        new Claim("gender", student.Gender ?? string.Empty),
        new Claim("dateOfBirth", student.DateOfBirth?.ToString("o") ?? string.Empty), 
        new Claim("GroupId", student.GroupId ?? string.Empty)
        // Thêm các claims khác nếu cần
    };

            return CreateToken(claims);
        }

        private string GenerateToken(Mentor mentor)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, mentor.MentorId),
        new Claim("accountType", "Mentor"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("fullName", mentor.MentorName),
        new Claim("email", mentor.Email),
        new Claim("phone", mentor.Phone ?? string.Empty),
        new Claim("gender", mentor.Gender ?? string.Empty),
        new Claim("dateOfBirth", mentor.DateOfBirth?.ToString("o") ?? string.Empty),
        new Claim("pointsReceived", mentor.PointsReceived?.ToString() ?? "0"),
        new Claim("numOfSlot", mentor.NumOfSlot?.ToString() ?? "0"),
        new Claim("registrationDate", mentor.RegistrationDate.ToString("o"))
    };

            return CreateToken(claims);
        }

        private string GenerateToken(Admin admin)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, admin.AdminId),
        new Claim("accountType", "Admin"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("email", admin.Email)
        // Thêm các claims khác nếu cần
    };

            return CreateToken(claims);
        }

        // Phương thức tạo token chung
        private string CreateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string GenerateRefreshToken(string userId)
        {
            var refreshToken = Guid.NewGuid().ToString(); // Tạo một GUID làm refresh token
            return refreshToken;
        }

        // Thêm phương thức refresh token
        public async Task<string> RefreshTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetRefreshTokenByUserIdAsync(token);
            if (refreshToken == null || refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new Exception("Refresh token không hợp lệ hoặc đã hết hạn.");
            }

            // Lấy thông tin người dùng dựa trên UserId
            var student = await _authRepository.GetStudentByIdAsync(refreshToken.UserId);
            if (student != null)
            {
                // Tạo access token mới cho Student
                return GenerateToken(student);
            }

            var mentor = await _authRepository.GetMentorByIdAsync(refreshToken.UserId);
            if (mentor != null)
            {
                // Tạo access token mới cho Mentor
                return GenerateToken(mentor);
            }

            // Nếu không tìm thấy người dùng, ném ra ngoại lệ
            throw new Exception("Người dùng không tồn tại.");
        }


    }
}
