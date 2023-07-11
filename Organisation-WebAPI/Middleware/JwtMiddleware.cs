using Microsoft.Extensions.Options;
using Organisation_WebAPI.Helpers;
using Organisation_WebAPI.Services.AuthRepo;

namespace Organisation_WebAPI.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthRepository _authRepository;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings,IAuthRepository authRepository)
        {
            _next = next;
            _authRepository = authRepository;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context,IAuthRepository authRepository,IJwtUtils jwtUtils ) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null) {
                //context.Items["User"] = 
                context.Items["User"] = _authRepository.GetUserById(userId.Value);
            }
            await _next(context);
        }
    }
}
