using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Organisation_WebAPI.Models
{
    public class Manager
    {
        public Manager()
        {
            Employees = new List<Employee>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ManagerId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ManagerName { get; set; }
        public string? Address { get; set; }
        public int ManagerSalary { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public int ManagerAge { get; set; }
        public User? User { get; set; }
        public bool IsAppointed { get; set; }
        public ICollection<Employee> Employees { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public Department Department { get; set; } = null!;

    }
}