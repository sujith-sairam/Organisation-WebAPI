using System.ComponentModel.DataAnnotations;

namespace Organisation_WebAPI.Models
{
    public class EmployeeTask
    {
        [Key]
        public int TaskID { get; set; }
        public string? TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime? TaskCreatedDate { get; set; }
        public DateTime? TaskDueDate { get; set; }
        public Status TaskStatus { get; set; }
        [ForeignKey("EmployeeID")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
