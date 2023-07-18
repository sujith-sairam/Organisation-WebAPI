namespace Organisation_WebAPI.Dtos.EmployeeDto
{
    public class GetEmployeesAndManagerDto
    {
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public int ManagerSalary { get; set; }
        public int ManagerAge { get; set; }
        public List<GetEmployeeDto> Employees { get; set; }
    }
}
