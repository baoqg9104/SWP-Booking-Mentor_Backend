using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP391_Mentor_Booking_System_Service.Service
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthService _authService;

        public JwtMiddleware(RequestDelegate next, AuthService authService)
        {
            _next = next;
            _authService = authService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lấy token từ header "Authorization"
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var isValid = await _authService.IsTokenValid(token);
                if (isValid)
                {
                    await _next(context);  // Nếu token hợp lệ, tiếp tục yêu cầu
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token không hợp lệ hoặc đã hết hạn.");
                }
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token không hợp lệ hoặc không có.");
            }
        }
    }
}