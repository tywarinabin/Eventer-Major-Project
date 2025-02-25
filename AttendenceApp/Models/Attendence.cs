using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendenceApp.Models;

public class Attendance
{
    [Key]
    public int Id { get; set; }

    public int EventId { get; set; }

    [ForeignKey("EventId")]
    public Event Event { get; set; } = null!;

    public int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    public Employee? Employee { get; set; } = null!;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
