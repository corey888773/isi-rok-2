using System.ComponentModel.DataAnnotations;

namespace Silownia.Models;

public class Wejscie {
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Data { get; set; }

    [Required]
    public Uzytkownik Uzytkownik { get; set; }

    [Required]
    public TimeSpan CzasTrwania { get; set; }
}