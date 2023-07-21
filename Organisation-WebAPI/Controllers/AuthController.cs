using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using organisation_webapi.dtos.admin;

using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Dtos.ManagerDto;
using Organisation_WebAPI.Models;
using Organisation_WebAPI.Services.AuthRepo;
using System.Data;
using System.Security.Claims;
using static System.Net.WebRequestMethods;


namespace Organisation_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(UserRole.Admin))]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }


        [HttpPost("Register")]
        [AllowAnonymous]
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
        [AllowAnonymous]

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

        
        [HttpGet("GetUserById")]
        
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserById(int id)
        {
            var response = await _authRepository.GetUserById(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<string>>> ResendOtp(string email)
        {
            var response = await _authRepository.ResendOtp(email);
            if (!response.Success)  
            {
                return BadRequest(response);
            }
            return Ok(response);
        }



    [HttpDelete("DeleteUserById")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<ActionResult<ServiceResponse<string>>> DeleteUserById(int id)
        {
            var response = await _authRepository.DeleteUserById(id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        public async Task<ActionResult<ServiceResponse<string>>> GetAllUsers()
        {
            var response = await _authRepository.GetAllUsers();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("AppointNewManager")]
        public async Task<ActionResult<ServiceResponse<string>>> AppointNewManager(NewManagerDto newManager, int id)
        {
            var response = await _authRepository.AppointNewManager(id, newManager);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
    