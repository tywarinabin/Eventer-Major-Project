using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AttendenceApp.Models
{
    public class EventFeedback
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
        public byte Rating { get; set; } 

        public string? Comments { get; set; }

        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}
