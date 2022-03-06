using System.ComponentModel.DataAnnotations;

namespace Models;

public class Registrations
{
    public string Id { get; set; }

    [Required]
    public string Club { get; set; }

    public IEnumerable<Registration> RegistrationList { get; set; }
}