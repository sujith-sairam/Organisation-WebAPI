using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organisation_WebAPI.DTO;
using Organisation_WebAPI.Models;
using Organisation_WebAPI.Repository.AuthRepo;

namespace Organisation_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(AdminRegisterDto request)
        {
            var response = await _authRepository.Register(
                new Admin { UserName = request.UserName }, request.Password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
