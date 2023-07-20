namespace Organisation_WebAPI.Dtos.EmployeeDto
{
    public class GetEmployeesAndManagerDto
    {
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int ManagerSalary { get; set; }
        public bool IsAppointed { get; set; }
        public int ManagerAge { get; set; }
        public string DepartmentName { get; set; } // Department name
        public List<GetEmployeeDto> Employees { get; set; }
    }
}
