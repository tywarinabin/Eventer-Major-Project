using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AttendenceApp.Models
{
    public class EventReport
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; } = null!;

        [Required]
        public string ReportType { get; set; } =null!;

        [Required]
        public string Description { get; set; } = null!;

        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
    }
}
