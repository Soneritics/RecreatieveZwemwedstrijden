using System.ComponentModel.DataAnnotations;

namespace Models;

public class Match
{
    public string Id { get; set; }

    [Required]
    public Date Date { get; set; }

    [Required]
    public Time Warmup { get; set; }

    [Required]
    public Time Start { get; set; }

    [Required]
    public string Organiser { get; set; }

    [Required]
    public string PoolName { get; set; }

    [Required]
    public string PoolAddress { get; set; }

    public IEnumerable<Program> Programs { get; set; }
}