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

        public string? ManagerName { get; set; }
        public int ManagerSalary { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }

        public int ManagerAge { get; set; }

        public User? User { get; set; }
        public bool isAppointed { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; } = null!;
    }
}