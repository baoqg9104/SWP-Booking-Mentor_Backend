using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Service.Service;
using SWP391_Mentor_Booking_System_Data.DTO;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                await _authService.RegisterAsync(registerDto);
                return Ok("Đăng ký thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                // Kiểm tra vai trò của người dùng từ LoginDTO
                (string accessToken, string refreshToken) tokens;

                if (loginDto.Role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    tokens = await _authService.LoginStudentAsync(loginDto);
                }
                else if (loginDto.Role.Equals("Mentor", StringComparison.OrdinalIgnoreCase))
                {
                    tokens = await _authService.LoginMentorAsync(loginDto);
                }
                else if (loginDto.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    tokens = await _authService.LoginAdminAsync(loginDto);
                }
                else
                {
                    return BadRequest("Vai trò không hợp lệ.");
                }

                // Trả về token sau khi đăng nhập thành công
                return Ok(new { AccessToken = tokens.accessToken, RefreshToken = tokens.refreshToken });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            try
            {
                var newAccessToken = await _authService.RefreshTokenAsync(token);
                return Ok(new { AccessToken = newAccessToken });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("validate-token")]
        public async Task<IActionResult> ValidateToken([FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token không được để trống.");
            }

            var isValid = _authService.IsTokenValid(token);
            if (isValid)
            {
                return Ok("Token hợp lệ.");
            }
            else
            {
                return Unauthorized("Token không hợp lệ."); 
            }
        }

    }
}
