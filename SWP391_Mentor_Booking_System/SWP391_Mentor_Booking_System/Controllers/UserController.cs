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

    }

}
