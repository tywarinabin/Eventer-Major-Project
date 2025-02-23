using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendenceApp.Models;

public class Event
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; } 

    [Required]
    public string EventType { get; set; } = null!;

    public DateTime Created { get; set; } = DateTime.UtcNow;

    public DateTime StartDate { get; set; }
    public int? RegistrationCount { get; set; } = 0;
    public int? ParticipationCount { get; set; } = 0;

    public DateTime? EndDate { get; set; } // Nullable if the event has no end date

    [NotMapped] // Prevents it from being a database column
    public int EventDuration => EndDate.HasValue ? (int)(EndDate.Value - StartDate).TotalDays : 0;

    public string Status { get; set; } = null!;


    public List<Attendance>? Attendances { get; set; } = new();
}
