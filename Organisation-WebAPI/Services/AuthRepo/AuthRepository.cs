using Azure;
using EmailService;
using Microsoft.EntityFrameworkCore;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Models;


namespace Organisation_WebAPI.Services.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly OrganizationContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly Dictionary<string, string> _otpDictionary;
        private readonly Dictionary<string, RegistrationData> _registeredUsers;

        public AuthRepository(OrganizationContext dbContext , IEmailSender emailSender)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
            _otpDictionary = new Dictionary<string, string>();
            _registeredUsers = new Dictionary<string, RegistrationData>();


        }
        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
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

            _otpDictionary.TryGetValue(email, out var existingOtp);
            _otpDictionary[email] = otp;

            _registeredUsers[email] = new RegistrationData() { 
                User = user,
                Password = password,
            };

            if (_otpDictionary.TryGetValue(email, out var storedOtp))
            {
                Console.WriteLine($"_otpDictionary: {email} - {_otpDictionary[email]}");
            }

            var message = new Message(new string[] { email }, $"Test email", $"This is your OTP : {otp}");
            _emailSender.SendEmail(message);

            //CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            //user.PasswordHash = passwordHash;
            //user.PasswordSalt = passwordSalt;
            //user.Email = email;

            //_dbContext.Admins.Add(user);
            //await _dbContext.SaveChangesAsync();

            response.Data = "Please check your email for OTP.";
            return response;
        }
        public async Task<ServiceResponse<string>> Verify(string email, string otp)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            //Console.WriteLine($"verify page _otpDictionary: {email} - {_otpDictionary[email]}");
            foreach (var value in _otpDictionary.Values)
            {
                Console.WriteLine(value);
            }


            if (!_otpDictionary.TryGetValue(email, out var storedOtp))
            {
                Console.WriteLine($"_otpDictionary: {email} - {_otpDictionary[email]}");

                response.Success = false;
                response.Message = "Invalid email or OTP expired.";
                return response;
            }

            if (otp == storedOtp)
            {
                if (_registeredUsers.TryGetValue(email, out var registrationData))
                {
                    // Perform any required verification checks on the user data and password
                    // ...
                    CreatePasswordHash(registrationData.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    registrationData.User.PasswordHash = passwordHash;
                    registrationData.User.PasswordSalt = passwordSalt;
                    registrationData.User.Email = email;
                    _dbContext.Admins.Add(registrationData.User);
                    await _dbContext.SaveChangesAsync();

                    // Remove the user, OTP, and registration data from dictionaries
                    _registeredUsers.Remove(email);
                    _otpDictionary.Remove(email);

                    response.Success = true;
                    response.Message = "OTP verification successful.";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to retrieve registration data";

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

    }
}
