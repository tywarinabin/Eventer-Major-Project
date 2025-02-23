using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendenceApp.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string EmployeeId { get; set; } = null!;

        [Required]
        [MaxLength(40)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(7)]
        [MaxLength(100)]
        public string Password { get; set; } = null!;
        public ICollection<Attendance>? Attendances { get; set; } = new List<Attendance>();
    }
}
