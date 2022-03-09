using System.ComponentModel.DataAnnotations;

namespace Models;

public class Registrations
{
    public string Id { get; set; }

    public string MatchId { get; set; }

    [Required]
    public string Club { get; set; }

    public IEnumerable<Registration> RegistrationList { get; set; }
}