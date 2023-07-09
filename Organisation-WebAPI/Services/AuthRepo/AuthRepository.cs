using Azure;
using EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;


namespace Organisation_WebAPI.Services.AuthRepo
{
    public class AuthRepository : IAuthRepository
    {
        private readonly OrganizationContext _dbContext;
        private readonly IEmailSender _emailSender;
        private readonly Dictionary<string, string> _otpDictionary;
        private readonly IConfiguration _configuration;
        private readonly IJwtUtils _jwtUtils;
        private readonly IMemoryCache _memoryCache;

        public AuthRepository(OrganizationContext dbContext, IConfiguration configuration,IJwtUtils jwtUtils, IEmailSender emailSender, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
            _otpDictionary = new Dictionary<string, string>();
            _configuration = configuration;
            _jwtUtils = jwtUtils;
            _memoryCache = memoryCache;
        }

       public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User Not Found";
            }
            else if (user.IsVerified == false)
            {
                response.Success = false;
                response.Message = "User Not Verified";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect Password";
                
            }
            else
            {
                response.Data = _jwtUtils.GenerateJwtToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> Register(UserRegisterDto model)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (!IsEmailValid(model.Email))
            {
                response.Success = false;
                response.Message = "Invalid email address.";
                return response;
            }

            if (await UserExists(model.UserName))
            {
                response.Success = false;
                response.Message = "User already exists";
                return response;
            }

            OtpGenerator otpGenerator = new OtpGenerator();
            string otp = otpGenerator.GenerateOtp();

            DateTimeOffset indianTime = DateTimeOffset.UtcNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").BaseUtcOffset);
            DateTimeOffset otpExpiration = indianTime.AddMinutes(2);

            CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Otp = otp,
                IsVerified = false,
                OtpExpiration = otpExpiration,
                Role = model.Role
            };

            await _dbContext.Users.AddAsync(user);

            if (user.Role == UserRole.Employee)
            {
                var employee = new Employee
                {
                    EmployeeName = model.EmployeeName,
                    EmployeeSalary = model.EmployeeSalary,
                    EmployeeAge = model.EmployeeAge,
                    DepartmentID = model.DepartmentID,
                    ProductID = model.ProductID,
                    User = user
                };

                await _dbContext.Employees.AddAsync(employee);
            }
            else if (user.Role == UserRole.Manager)
            {
                var manager = new Manager
                {
                    ManagerName = model.ManagerName,
                    ManagerSalary = model.ManagerSalary,
                    ManagerAge = model.ManagerAge,
                    ProductID = model.ProductID,
                    User = user
                };

                await _dbContext.Managers.AddAsync(manager);
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid user role.";
                return response;
            }

            await _dbContext.SaveChangesAsync();

            var message = new Message(new string[] { model.Email }, "HR GO - OTP", $"Your OTP for registering in HR GO Portal is: {otp}.\n\nIt will expire at {otpExpiration} IST.");
            _emailSender.SendEmail(message);

            response.Data = "Please check your email for OTP.";
            return response;
        }


        public async Task<ServiceResponse<string>> Verify(string email, string otp)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();

            if (!IsEmailValid(email))
            {
                response.Success = false;
                response.Message = "Invalid email address.";
                return response;
            }
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (user.Otp != null && user.OtpExpiration > DateTimeOffset.UtcNow) { 
                    user.IsVerified = true;
                    user.Otp = null;
                    await _dbContext.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "OTP verification successful.";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid OTP , Please try again";

                }

            }
            else {

                response.Success = false;
                response.Message = "Invalid Email , Please try again";

            }

            return response;
        }

      


        public async Task<bool> UserExists(string username)
        {
            if (await _dbContext.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }
        public async Task<ServiceResponse<string>> ForgotPassword(string email)
        {
            var response = new ServiceResponse<string>();
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (!IsEmailValid(email))
            {
                response.Success = false;
                response.Message = "Invalid email address.";
                return response;
            }
            if (!await EmailExists(email))
            {
                response.Success = false;
                response.Message = "Email does not exists";
                return response;
            }
            if (user.IsVerified == false)
            {
                response.Success = false;
                response.Message = "User Not Verified";
            }

            OtpGenerator otpGenerator = new OtpGenerator();
            string otp = otpGenerator.GenerateOtp();

            // Get the current Indian time
            DateTimeOffset indianTime = DateTimeOffset.UtcNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").BaseUtcOffset);

            // Add the expiration time in minutes
            DateTimeOffset otpExpiration = indianTime.AddMinutes(3);

            user.Otp = otp;
            user.OtpExpiration = otpExpiration;

            await _dbContext.SaveChangesAsync();

            var message = new Message(new string[] { email }, $"Forgot Password OTP", $"This is your OTP to reset your password : {otp}.\n\nIt will expire at {otpExpiration} IST.");
            _emailSender.SendEmail(message);
            response.Data = "Please check your email for OTP.";
            return response;
        }



        public async Task<ServiceResponse<ResetPasswordDto>> ResetPassword(ResetPasswordDto request)
        {
            ServiceResponse<ResetPasswordDto> response = new ServiceResponse<ResetPasswordDto>();

            if (request.Email is not null) {

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
                
                if(user != null)
                {
                    if (user.IsVerified == false)
                    {
                        response.Success = false;
                        response.Message = "User Not Verified";
                        return response;
                    }

                    if (user.OtpExpiration > DateTimeOffset.UtcNow)
                    {
                        CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                        user.PasswordHash = passwordHash;
                        user.PasswordSalt = passwordSalt;
                        user.Otp = null;
                        user.OtpExpiration = null;
                        await _dbContext.SaveChangesAsync();
                        response.Message = "Password changed successfully.";

                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Your OTP is expired.";
                        return response;
                    }

                }
                else{
                    response.Success = false;
                    response.Message = "Invalid Email Address.";
                }
              
            }
            else
            {
                response.Success = false;
                response.Message = "Email is Required.";
            }

            return response;

        }


        public async Task<ServiceResponse<string>> ResendOtp(string email)
        {
            var response = new ServiceResponse<string>();

            if (!IsEmailValid(email))
            {
                response.Success = false;
                response.Message = "Invalid email address.";
                return response;
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                response.Success = false;
                response.Message = "Email does not exist.";
                return response;
            }

            if (user.OtpResendCount >= 3)
            {
                response.Success = false;
                response.Message = "Maximum OTP resend limit reached.";
                return response;
            }

            OtpGenerator otpGenerator = new OtpGenerator();
            string otp = otpGenerator.GenerateOtp();

            // Get the current Indian time
            DateTimeOffset indianTime = DateTimeOffset.UtcNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").BaseUtcOffset);

            // Add the expiration time in minutes
            DateTimeOffset otpExpiration = indianTime.AddMinutes(3);

            user.Otp = otp;
            user.OtpExpiration = otpExpiration;
            user.OtpResendCount++;

            await _dbContext.SaveChangesAsync();

            var message = new Message(new string[] { email }, "OTP Resent", $"This is your new OTP: {otp}.\n\nIt will expire at {otpExpiration} IST.");
            _emailSender.SendEmail(message);

            response.Success = true;
            response.Message = "OTP has been resent.";
            return response;
        }



        public async Task<bool> EmailExists(string email)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()))
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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private bool IsEmailValid(string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            // Check if the email matches the pattern
            bool isValid = Regex.IsMatch(email, pattern);

            return isValid;
        }

    }
}
