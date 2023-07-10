using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using organisation_webapi.dtos.admin;
using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Models;
using Organisation_WebAPI.Services.AuthRepo;
using static System.Net.WebRequestMethods;

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

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _authRepository.Register(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(request.UserName, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Verify")]
        public async Task<ActionResult<ServiceResponse<string>>> Verify(string email, string otp)
        {
            var response = await _authRepository.Verify(
                email, otp
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


        [HttpPost("GetUserById")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserById(int id)
        {
            var response = await _authRepository.GetUserBYId(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ForgotPassword(string email)
        {
            var response = await _authRepository.ForgotPassword(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("ResetPassword")]
        public async Task<ActionResult<ServiceResponse<string>>> ResetPassword(ResetPasswordDto request)
        {
            var response = await _authRepository.ResetPassword(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost("ResendOtp")]
        public async Task<ActionResult<ServiceResponse<string>>> ResendOtp(string email)
        {
            var response = await _authRepository.ResendOtp(email);
            if (!response.Success)  
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
