using Organisation_WebAPI.Models;

namespace Organisation_WebAPI.Repository.AuthRepo
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(Admin user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
