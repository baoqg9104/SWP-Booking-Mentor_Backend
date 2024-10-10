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

        [HttpPost("login/student")]
        public async Task<IActionResult> LoginStudent([FromBody] LoginDTO loginDto)
        {
            try
            {
                var (accessToken, refreshToken) = await _authService.LoginStudentAsync(loginDto);
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login/mentor")]
        public async Task<IActionResult> LoginMentor([FromBody] LoginDTO loginDto)
        {
            try
            {
                var (accessToken, refreshToken) = await _authService.LoginMentorAsync(loginDto);
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login/admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDTO loginDto)
        {
            try
            {
                var (accessToken, refreshToken) = await _authService.LoginAdminAsync(loginDto);
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
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



    }
}
