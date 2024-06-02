using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UiDoAn.Controllers;

namespace UiDoAn.Models.Middleware
{
    public class UserNameFilterMiddleware
    {
        private readonly RequestDelegate _next;

        public UserNameFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Thực hiện các hoạt động trước khi điều hướng đến middleware tiếp theo
            context.Items["NameUser"] = AuthenticateController.NameUser;

            // Gọi middleware tiếp theo trong pipeline
            await _next(context);
        }
    }
}
