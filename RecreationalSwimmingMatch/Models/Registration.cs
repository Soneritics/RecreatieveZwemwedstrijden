using System.ComponentModel.DataAnnotations;

namespace Models;

public class Registration
{
    public string Id { get; set; }

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    [Required]
    public int YearOfBirth { get; set; }

    public IEnumerable<string> ProgramIds { get; set; }
}