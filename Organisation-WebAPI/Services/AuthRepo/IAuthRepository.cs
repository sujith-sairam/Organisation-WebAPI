using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Models;

namespace Organisation_WebAPI.Services.AuthRepo
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<string>> Register(UserRegisterDto model);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<string>> Verify(string email, string otp);
        Task<ServiceResponse<string>> ForgotPassword(string email);
        Task<ServiceResponse<ResetPasswordDto>> ResetPassword(ResetPasswordDto email);
        Task<ServiceResponse<string>> ResendOtp(string email);
        Task<bool> UserExists(string username);

    }
}
