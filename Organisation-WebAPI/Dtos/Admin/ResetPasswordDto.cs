namespace Organisation_WebAPI.Dtos.Admin
{
    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Otp { get; set; }

    }
}
