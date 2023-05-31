using System.ComponentModel.DataAnnotations;

namespace Silownia.Models;

public class Karnet {
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nazwa { get; set; }

    [Required]
    public decimal Cena { get; set; }

    public string Opis { get; set; }

    public int CzasTrwaniaWMiesiacach { get; set; }

    public ICollection<Uzytkownik>? Uzytkownicy { get; set; }
}
