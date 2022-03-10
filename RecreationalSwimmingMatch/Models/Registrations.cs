using System.ComponentModel.DataAnnotations;

namespace Models;

public class Registrations
{
    public string Id { get; set; }

    public string MatchId { get; set; }

    [Required]
    public string Club { get; set; }

    public IEnumerable<Registration> RegistrationList { get; set; }

    public void CleanRegistrationList()
    {
        if (RegistrationList?.Any() == true)
        {
            RegistrationList = RegistrationList
                .Where(r => !string.IsNullOrEmpty(r.Firstname) && !string.IsNullOrEmpty(r.Lastname))
                .OrderBy(r => r.Firstname).ThenBy(r => r.Lastname);
        }
    }
}