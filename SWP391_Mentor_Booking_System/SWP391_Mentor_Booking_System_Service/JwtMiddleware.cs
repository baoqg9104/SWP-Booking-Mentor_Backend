using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using SWP391_Mentor_Booking_System_Service.Service;



    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Lấy token từ Header Authorization
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    // Lấy AuthService từ RequestServices (Scoped Service)
                    var authService = context.RequestServices.GetRequiredService<AuthService>();

                    // Xác thực token
                    var isValid = await authService.IsTokenValid(token);

                    if (!isValid)
                    {
                        // Nếu token không hợp lệ, trả về lỗi 401 Unauthorized
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Token không hợp lệ hoặc đã hết hạn.");
                        return;
                    }
                }
                catch
                {
                    // Trả về lỗi 500 nếu có vấn đề nội bộ
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("Có lỗi xảy ra khi xác thực token.");
                    return;
                }
            }

            // Nếu không có token, tiếp tục xử lý request mà không cần xác thực
            await _next(context);
        }
    
}
