using System.ComponentModel.DataAnnotations;

namespace Silownia.Models;

public class Uzytkownik
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Haslo { get; set; }

    public string Imie { get; set; }

    public string Nazwisko { get; set; }

    public Karnet? Karnet { get; set; }

    public ICollection<Wejscie>? Wejscia { get; set; }

    public bool CzyAdministrator { get; set; }

    public bool CzyPracownik { get; set; }
}
