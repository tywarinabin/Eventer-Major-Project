using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string EmployeeID { get; set; } = null!;

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public int NumberOfEventsParticipated { get; set; } = 0;

    // Relationship with Attendance
    public List<Attendance> Attendances { get; set; } = new List<Attendance>();
}
