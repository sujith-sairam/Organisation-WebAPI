namespace Organisation_WebAPI.Dtos.Admin
{
    public class GetUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; } // Role field using the UserRole enum
    }
}
