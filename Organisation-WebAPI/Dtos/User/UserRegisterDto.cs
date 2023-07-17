namespace Organisation_WebAPI.Dtos.Admin
{
    public class UserRegisterDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; } // Role field using the UserRole enum

        // Employee-specific fields
        public string EmployeeName { get; set; }
        public int EmployeeSalary { get; set; }
        public int EmployeeAge { get; set; }
        public int DepartmentID { get; set; }
        public int ProductID { get; set; }
        // Manager-specific fields
        public string ManagerName { get; set; }
        public int ManagerSalary { get; set; }
        public int ManagerAge { get; set; }


    }
}
