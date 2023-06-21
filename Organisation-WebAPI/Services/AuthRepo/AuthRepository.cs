using Azure;
using EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.WebRequestMethods;


namespace Organisation_WebAPI.Services.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly OrganizationContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly Dictionary<string, string> _otpDictionary;
        private readonly Dictionary<string, RegistrationData> _registeredUsers;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public AuthRepository(OrganizationContext dbContext, IConfiguration configuration, IEmailSender emailSender, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
            _otpDictionary = new Dictionary<string, string>();
            _registeredUsers = new Dictionary<string, RegistrationData>();
            _configuration = configuration;
            _memoryCache = memoryCache;

        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _dbContext.Admins.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User Not Found";
            }

            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect Password";
            }
            else
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> Register(Admin user, string password, string email)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            if (await UserExists(user.UserName))
            {
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }
            OtpGenerator otpGenerator = new OtpGenerator();
            string otp = otpGenerator.GenerateOtp();


            RegistrationData registrationData = new RegistrationData
            {
                User = user,
                Password = password
            };

            _memoryCache.Set("OTP", otp);

            _memoryCache.Set(email, registrationData);


            var message = new Message(new string[] { email }, $"Test email", $"This is your OTP : {otp}");
            _emailSender.SendEmail(message);

            response.Data = "Please check your email for OTP.";
            return response;
        }
        public async Task<ServiceResponse<string>> Verify(string email, string otp)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (!_memoryCache.TryGetValue("OTP", out var storedOtp))
            {
                // OTP not found, handle accordingly
                response.Success = false;
                response.Message = "Invalid email or OTP expired.";
                return response;
            }
            else { 
                Console.WriteLine($"OTP for {email}: {storedOtp} -   {otp}");
            
            }


            if (otp == storedOtp.ToString())
            {
                if (_memoryCache.TryGetValue(email, out RegistrationData registrationData))
                {
                    // Perform any required verification checks on the user data and password
                    // ...
                    CreatePasswordHash(registrationData.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    registrationData.User.PasswordHash = passwordHash;
                    registrationData.User.PasswordSalt = passwordSalt;
                    registrationData.User.Email = email;
                    _dbContext.Admins.Add(registrationData.User);
                    await _dbContext.SaveChangesAsync();

                    _memoryCache.Remove(email);
                    _memoryCache.Remove("OTP");

                    response.Success = true;
                    response.Message = "OTP verification successful.";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Incorrect Email, Please try again later";

                }

            }
            else {
                response.Success = false;
                response.Message = "Invalid OTP , Please try again";

            }

            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dbContext.Admins.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }
        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            var response = new ServiceResponse<string>();
            var user = await _dbContext.Admins.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (!await EmailExists(email))
            {
                response.Success = false;
                response.Message = "Invalid Email";
                return response;
            }

            return response;

        }

        public async Task<bool> EmailExists(string email)
        {
            if (await _dbContext.Admins.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        private class RegistrationData
        {
            public Admin User { get; set; }
            public string Password { get; set; }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }


        private string CreateToken(Admin admin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.UserName)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        
    }
}
