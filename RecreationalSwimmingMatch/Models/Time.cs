using System.ComponentModel.DataAnnotations;

namespace Models;

public class Time
{
    [Required]
    public int Hour { get; set; }

    [Required]
    public int Minute { get; set; }
}