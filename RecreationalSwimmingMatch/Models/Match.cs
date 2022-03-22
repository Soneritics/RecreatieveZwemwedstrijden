using System.ComponentModel.DataAnnotations;

namespace Models;

public class Match
{
    public string Id { get; set; }

    public DateTime LastSaved { get; set; }

    [Required]
    public string Name { get; set; }

    public string Logo { get; set; }

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

    [Required]
    public int SwimmingLanes { get; set; } = 5;

    public int BreakAfterProgram { get; set; }

    public IEnumerable<Program> Programs { get; set; }

    public void CleanPrograms()
    {
        if (Programs?.Any() == true)
        {
            var programList = new List<Program>();
            var order = 0;

            foreach (var program in Programs.OrderBy(p => p.Order))
            {
                if (!string.IsNullOrEmpty(program.Stroke))
                {
                    program.Order = ++order;
                    programList.Add(program);
                }
            }

            Programs = programList;
        }
    }
}