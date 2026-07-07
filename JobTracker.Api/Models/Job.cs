using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.Models;

// public enum ApplicationStatus
// {
//     Applied,
//     Interviewing,
//     Offer,
//     Rejected
// }

// Models are classes that define our entities
public class Job
{
    [Key]
    public int Id { get; set; }

    // Other attributes
    public string Company { get; set; } = "";
    public string Role { get; set; } = "";
    public string Location { get; set; } = "";
    public string Status { get; set; } = string.Empty;
    public DateTime DateApplied { get; set; }
    public string? Notes { get; set; }
}