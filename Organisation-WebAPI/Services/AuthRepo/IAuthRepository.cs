using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Models;

namespace Organisation_WebAPI.Services.AuthRepo
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<string>> Register(Admin user, string password, string email);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<string>> Verify(string email, string otp);
        Task<ServiceResponse<string>> ForgotPassword(string email);
        Task<ServiceResponse<ResetPasswordDto>> ResetPassword(ResetPasswordDto email);
        Task<bool> UserExists(string username);

    }
}
