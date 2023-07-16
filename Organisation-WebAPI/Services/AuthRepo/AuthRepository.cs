using AutoMapper;
using Azure;
using EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Organisation_WebAPI.Data;
using Organisation_WebAPI.Dtos.Admin;
using Organisation_WebAPI.Dtos.CustomerDto;
using Organisation_WebAPI.Dtos.DepartmentDto;
using Organisation_WebAPI.Dtos.ManagerDto;
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
        private readonly IMapper _mapper;


        public AuthRepository(OrganizationContext dbContext, IConfiguration configuration,IJwtUtils jwtUtils, IEmailSender emailSender, IMemoryCache memoryCache, IMapper mapper)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
            _otpDictionary = new Dictionary<string, string>();
            _configuration = configuration;
            _jwtUtils = jwtUtils;
            _memoryCache = memoryCache;
            _mapper = mapper;
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

            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Incorrect Password";
                
            }
            else
            {
                if (user.Role == UserRole.Manager) {
                    var manager = await _dbContext.Managers.FindAsync(user.UserID);
                    if (manager.isAppointed == false) { 
                        response.Success = false;
                        response.Message = "User Not Found";
                        return response;
                    }
                }
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

            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = new User
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        Role = model.Role,
                        IsVerified = true
                    };

                    await _dbContext.Users.AddAsync(user);
                    await _dbContext.SaveChangesAsync();

                    if (model.Role == UserRole.Employee)
                    {
                        var department = await _dbContext.Departments.FindAsync(model.DepartmentID);
                        var manager = await _dbContext.Managers.FindAsync(model.ManagerID);

                        if (department == null)
                        {
                            response.Success = false;
                            response.Message = "Invalid department ID. Department Not Found";
                            transaction.Rollback();
                            return response;
                        }

                        if (manager == null)
                        {
                            response.Success = false;
                            response.Message = "Invalid manager ID. Manager Not Found";
                            transaction.Rollback();
                            return response;
                        }

                        var employee = new Employee
                        {
                            EmployeeID = user.UserID,
                            EmployeeName = model.EmployeeName,
                            EmployeeSalary = model.EmployeeSalary,
                            EmployeeAge = model.EmployeeAge,
                            DepartmentID = model.DepartmentID,
                            ManagerID = model.ManagerID,
                            User = user
                        };
                        // Send email to employee
                        var employeeMessage = new Message(new string[] { model.Email }, "Welcome to ORG 360 - Employee Registration",
                            $"Dear {model.UserName},\n\nCongratulations! You have been registered as an employee in ORG 360." +
                            $"\n\nYour credentials:\nUsername: {model.UserName}\nPassword: {model.Password}\n\nPlease keep this " +
                            $"information confidential.\n\nThank you and welcome to ORG 360!");

                        _emailSender.SendEmail(employeeMessage);

                        await _dbContext.Employees.AddAsync(employee);
                    }
                    else if (model.Role == UserRole.Manager)
                    {
                        var existingManager = await _dbContext.Managers.FirstOrDefaultAsync(m => m.ProductID == model.ProductID);
                        if (existingManager != null)
                        {
                            response.Success = false;
                            response.Message = "Product ID is already associated with a manager.";
                            transaction.Rollback();
                            return response;
                        }

                        var manager = new Manager
                        {
                            ManagerId = user.UserID,
                            ManagerName = model.ManagerName,
                            ManagerSalary = model.ManagerSalary,
                            ManagerAge = model.ManagerAge,
                            ProductID = model.ProductID,
                            isAppointed = true,
                            User = user
                        };

                        var managerMessage = new Message(new string[] { model.Email }, "Welcome to ORG 360 - Manager Registration",
                            $"Dear {model.UserName},\n\nCongratulations! You have been registered as a manager in ORG 360." +
                            $"\n\nYour credentials:\nUsername: {model.UserName}\nPassword: {model.Password}\n\nPlease keep this " +
                            $"information confidential.\n\nThank you and welcome to ORG 360!");

                        _emailSender.SendEmail(managerMessage);

                        await _dbContext.Managers.AddAsync(manager);
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "Invalid user role.";
                        return response;
                    }

                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();

                    response.Data = "User Registrated successfully";
                    return response;
                }
                catch (Exception ex)
                {
                    // Handle exception if needed
                    transaction.Rollback();
                    response.Success = false;
                    response.Message = "An error occurred during registration.";
                    return response;
                }
            }
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
                if (user.OtpResendCount >= 3)
                {
                    response.Success = false;
                    response.Message = "Maximum OTP resend limit reached.";
                    return response;
                }
                if (user.Otp == otp) {

                    if (user.OtpExpiration > DateTimeOffset.UtcNow)
                    {
                        user.Otp = null;
                        user.IsVerified = true;
                        await _dbContext.SaveChangesAsync();
                        response.Success = true;
                        response.Message = "OTP verification successful.";
                        return response;
                    }
                    else {
                        response.Success = false;
                        response.Message = "Your Otp has been expired , Please try again";
                    }  
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
            

            OtpGenerator otpGenerator = new OtpGenerator();
            string otp = otpGenerator.GenerateOtp();

            // Get the current Indian time
            DateTimeOffset indianTime = DateTimeOffset.UtcNow.ToOffset(TimeZoneInfo.FindSystemTimeZoneById("India Standard Time").BaseUtcOffset);

            // Add the expiration time in minutes
            DateTimeOffset otpExpiration = indianTime.AddMinutes(3);

            user.Otp = otp;
            user.OtpExpiration = otpExpiration;
            user.IsVerified = false;
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

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            var response = new ServiceResponse<GetUserDto>();
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            response.Data = _mapper.Map<GetUserDto>(user); ;
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteUserById(int id)
        {
            var response = new ServiceResponse<string>();

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            if (user.Role == UserRole.Employee)
            {
                _dbContext.Users.Remove(user);

            }
            else if (user.Role == UserRole.Manager)
            {
                // Update the manager's IsAppointed property to false
                var manager = await _dbContext.Managers.FirstOrDefaultAsync(m => m.ManagerId == id);
                if (manager != null)
                {
                    manager.isAppointed = false;
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Invalid user role";
                return response;
            }

     
            await _dbContext.SaveChangesAsync();

            response.Data = "User deleted successfully";
            return response;

        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetAllUsers()
        {
            var response = new ServiceResponse<List<GetUserDto>>();
            var users = await _dbContext.Users.ToListAsync();

            response.Data = users.Select(c => _mapper.Map<GetUserDto>(c)).ToList();
            response.Message = "Users retrieved successfully";

            return response;

        }


        public async Task<ServiceResponse<string>> AppointNewManager(int managerId, NewManagerDto model)
        {
            var response = new ServiceResponse<string>();

            var manager = await _dbContext.Managers.Include(m => m.User)
                                                  .FirstOrDefaultAsync(m => m.ManagerId == managerId);

            if (manager == null)
            {
                response.Success = false;
                response.Message = "Manager not found";
                return response;
            }

            // Update the user associated with the manager
            var user = await _dbContext.Users.FindAsync(managerId);

            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);


            // Update user properties
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.IsVerified = true;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            // Update other user fields as needed

            // Update manager properties

            manager.ManagerName = model.ManagerName;
            manager.ManagerSalary = model.ManagerSalary;
            manager.ManagerAge = model.ManagerAge;
            manager.isAppointed = true;
            manager.User = user;

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Send email with updated information
            var message = new Message(new string[] { user.Email }, "Manager Information Updated", "Your manager information has been updated.");
            _emailSender.SendEmail(message);

            response.Data = "Manager updated successfully";
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
            user.IsVerified = false;
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
