namespace Organisation_WebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string? Otp { get; set; }
        public DateTimeOffset? OtpExpiration { get; set; }
        public int OtpResendCount { get; set; } = 0;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserRole Role { get; set; } // Role field using the UserRole enum

    }
}
