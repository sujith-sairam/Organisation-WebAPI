using Microsoft.Extensions.Options;
using Organisation_WebAPI.Services.AuthRepo;

namespace Organisation_WebAPI.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthRepository _authRepository;

        public JwtMiddleware(RequestDelegate next,IAuthRepository authRepository)
        {
            _next = next;
            _authRepository = authRepository;
        }

        public async Task Invoke(HttpContext context,IAuthRepository authRepository,IJwtUtils jwtUtils ) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token!);
            if (userId != null) {
                //context.Items["User"] = 
                context.Items["User"] = _authRepository.GetUserById(userId.Value);
            }
            await _next(context);
        }
    }
}
