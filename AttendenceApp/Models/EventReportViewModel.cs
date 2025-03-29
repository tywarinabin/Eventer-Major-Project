using System.ComponentModel.DataAnnotations;

public class EventReportViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Event ID is required.")]
    public int EventId { get; set; }

    public Event? Event { get; set; } // Optional: To display event details

    [Required(ErrorMessage = "Employee ID is required.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Report type is required.")]
    [StringLength(50, ErrorMessage = "Report type must be less than 50 characters.")]
    public string ReportType { get; set; } = null!;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
    public string Description { get; set; } = null!;

    public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;

    // Optional: Success and error messages for the view
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Status { get; set; }
    public string? ResolutionNotes { get; set; }
}