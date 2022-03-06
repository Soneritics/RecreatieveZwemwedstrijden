using System.ComponentModel.DataAnnotations;

namespace Models;

public class Date
{
    [Required]
    public int Day { get; set; }

    [Required]
    public int Month { get; set; }

    [Required]
    public int Year { get; set; }
}