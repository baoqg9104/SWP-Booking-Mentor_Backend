using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWP391_Mentor_Booking_System_Data;
using SWP391_Mentor_Booking_System_Data.Data;
using SWP391_Mentor_Booking_System_Data.DTO;
using SWP391_Mentor_Booking_System_Data.Repositories;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class AuthService
    {
        private readonly AuthRepository _authRepository;
        private readonly RefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;

        public AuthService(
                AuthRepository authRepository,
                IConfiguration configuration,
                RefreshTokenRepository refreshTokenRepository
            )
        {
            _authRepository = authRepository;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"])
            );
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
                    SwpClassId = registerDto.SwpClassId,
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
                    PointsReceived = 0,
                    NumOfSlot = 10,
                    RegistrationDate = DateTime.Now,
                    SwpClassId = null,

                    ApplyStatus =
                        false // Đặt ApplyStatus là false
                    ,
                    // Các trường khác để null
                };
                _authRepository.AddMentor(mentor);
            }
            else
            {
                throw new Exception("Invalid role");
            }

            await _authRepository.SaveChangesAsync(); // Đảm bảo gọi phương thức SaveChangesAsync từ repository
        }

        // Phương thức đăng nhập cho Student
        public async Task<(string accessToken, string refreshToken)> LoginStudentAsync(
            LoginDTO loginDto
        )
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
            var accessToken = CreateToken(student, "Student");
            var refreshToken = GenerateRefreshToken(student.StudentId);

            //luu
            await SaveAccessTokenAsync(student.StudentId, accessToken);

            // Lưu refresh token vào cơ sở dữ liệu
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = student.StudentId,
                ExpiryDate = DateTime.UtcNow.AddDays(
                    7
                ) // Thời gian hết hạn 7 ngày
                ,
            };
            _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        // Phương thức đăng nhập cho Mentor
        public async Task<(string accessToken, string refreshToken)> LoginMentorAsync(
            LoginDTO loginDto
        )
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

            //if (!mentor.ApplyStatus)
            //{
            //    throw new Exception("Đợi admin approved.");
            //}

            var accessToken = CreateToken(mentor, "Mentor");
            var refreshToken = GenerateRefreshToken(mentor.MentorId);

            //luu
            await SaveAccessTokenAsync(mentor.MentorId, accessToken);

            // Lưu refresh token vào cơ sở dữ liệu
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = mentor.MentorId,
                ExpiryDate = DateTime.UtcNow.AddDays(
                    7
                ) // Hết hạn sau 7 ngày
                ,
            };
            _refreshTokenRepository.AddRefreshToken(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        // Phương thức đăng nhập cho Admin
        public async Task<(string accessToken, string refreshToken)> LoginAdminAsync(
            LoginDTO loginDto
        )
        {
            var admin = await _authRepository.GetAdminByEmailAsync(loginDto.Email);

            if (admin == null)
            {
                throw new Exception("Email không tồn tại.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.Password))
            {
                throw new Exception("Mật khẩu không chính xác.");
            }

            // Tạo token cho admin
            var accessToken = CreateToken(admin, "Admin");
            var refreshToken = GenerateRefreshToken(admin.AdminId);

            await SaveAccessTokenAsync(admin.AdminId, accessToken);

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = admin.AdminId,
                ExpiryDate = DateTime.UtcNow.AddDays(
                    7
                ) // Hết hạn sau 7 ngày
                ,
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

        private string CreateToken<T>(T user, string role)
            where T : class
        {
            var claims = new List<Claim>();

            // Kiểm tra các thuộc tính cụ thể của user và thêm vào claims
            if (typeof(T) == typeof(Student))
            {
                var student = user as Student;
                claims.Add(new Claim("id", student.StudentId));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, student.Email));
                claims.Add(new Claim("fullName", student.StudentName));
                claims.Add(new Claim("groupId", student.GroupId ?? string.Empty));
                claims.Add(new Claim("swpClassId", student.SwpClassId == null ? string.Empty : student.SwpClassId.ToString()));
            }
            else if (typeof(T) == typeof(Mentor))
            {
                var mentor = user as Mentor;
                claims.Add(new Claim("id", mentor.MentorId));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, mentor.Email));
                claims.Add(new Claim("fullName", mentor.MentorName));
                claims.Add(new Claim("swpClassId", mentor.SwpClassId == null ? string.Empty : mentor.SwpClassId.ToString()));
            }
            else if (typeof(T) == typeof(Admin))
            {
                var admin = user as Admin;
                claims.Add(new Claim("id", admin.AdminId));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, admin.Email));
                claims.Add(new Claim("fullName", admin.AdminName));
            }

            claims.Add(new Claim("role", role));

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken(string userId)
        {
            var refreshToken = Guid.NewGuid().ToString(); // Tạo một GUID làm refresh token
            return refreshToken;
        }

        // Thêm phương thức refresh token
        public async Task<string> RefreshTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetRefreshTokenByTokenAsync(token); // Lấy refresh token từ DB bằng token
            if (refreshToken == null)
            {
                throw new Exception("Refresh token không hợp lệ.");
            }

            // Ghi log để kiểm tra ExpiryDate
            Console.WriteLine(
                $"ExpiryDate: {refreshToken.ExpiryDate}, CurrentTime: {DateTime.UtcNow}"
            );

            if (refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new Exception("Refresh token đã hết hạn.");
            }

            // Lấy thông tin người dùng dựa trên UserId
            var student = await _authRepository.GetStudentByIdAsync(refreshToken.UserId);
            if (student != null)
            {
                // Tạo access token mới cho Student
                return CreateToken(student, "Student");
            }

            var mentor = await _authRepository.GetMentorByIdAsync(refreshToken.UserId);
            if (mentor != null)
            {
                // Tạo access token mới cho Mentor
                return CreateToken(mentor, "Mentor");
            }

            // Nếu không tìm thấy người dùng, ném ra ngoại lệ
            throw new Exception("Người dùng không tồn tại.");
        }
        private async Task SaveAccessTokenAsync(string userId, string accessToken)
        {
            // Xóa các AccessToken cũ của User
            var existingTokens = await _authRepository.GetActiveTokensByUserIdAsync(userId);
            _authRepository.RemoveActiveTokens(existingTokens);

            // Thêm token mới
            var activeToken = new ActiveToken
            {
                TokenId = accessToken,
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(7) // Token hết hạn sau 7 ngày
            };

            _authRepository.AddActiveToken(activeToken);
            await _authRepository.SaveChangesAsync();
        }

        public async Task LogoutAsync(string accessToken)
        {
            var activeToken = await _authRepository.GetActiveTokenByTokenAsync(accessToken);
            if (activeToken != null)
            {
                _authRepository.RemoveActiveTokens(new List<ActiveToken> { activeToken });
                await _authRepository.SaveChangesAsync();
            }
        }


        public async Task<bool> IsTokenValid(string token)
        {
            // Kiểm tra token trong bảng ActiveTokens hoặc RefreshTokens
            var activeToken = await _authRepository.GetActiveTokenByTokenAsync(token);
            return activeToken != null && activeToken.ExpiryDate > DateTime.UtcNow;
        }

    }
}
