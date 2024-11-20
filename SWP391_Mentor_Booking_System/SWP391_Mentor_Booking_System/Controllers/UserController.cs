using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_Mentor_Booking_System_Data.DTO;
using SWP391_Mentor_Booking_System_Service.Service;

namespace SWP391_Mentor_Booking_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPut("update-user")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO updateUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var (result, error) = await _userService.UpdateUserAsync(updateUserDto);

                if (!result)
                    return NotFound(error);

                return Ok("Update User Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("change-password")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassDTO changePassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ChangePasswordAsync(changePassDto);
            if (!result)
                return NotFound();

            return Ok("Update Password Successfully");
        }

        [HttpPost("generate-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateOtp([FromBody] EmailRequest emailRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _userService.GenerateAndSendOtpAsync(emailRequest.RecipientEmail);

            if (!success)
                return BadRequest(error);

            return Ok("OTP sent successfully.");
        }

        [HttpPost("validate-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateOtp([FromBody] OtpRequest otpRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _userService.ValidateOtpAsync(otpRequest.Email, otpRequest.Otp);

            if (!success)
                return BadRequest($"Error: {error}");

            return Ok("OTP validated successfully.");
        }

        [HttpPost("set-new-password")]
        [AllowAnonymous]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, error) = await _userService.SetNewPasswordAsync(dto);

            if (!success)
                return BadRequest(error);

            return Ok("Set new password successful");
        }
    }

    public class EmailRequest
    {
        public string RecipientEmail { get; set; }
    }

    public class OtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
