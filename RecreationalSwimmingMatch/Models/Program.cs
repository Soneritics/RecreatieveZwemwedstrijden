using System.ComponentModel.DataAnnotations;

namespace Models;

public class Program
{
    [Required]
    public string Id { get; set; }

    public int Order { get; set; }

    [Required]
    public int Distance { get; set; }

    [Required]
    public string Stroke { get; set; }

    public string Comments { get; set; }
}