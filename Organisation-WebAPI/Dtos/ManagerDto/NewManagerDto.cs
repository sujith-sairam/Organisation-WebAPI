namespace Organisation_WebAPI.Dtos.ManagerDto
{
    public class NewManagerDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ManagerName { get; set; }
        public int ManagerSalary { get; set; }
        public int ManagerAge { get; set; }
    }
}
