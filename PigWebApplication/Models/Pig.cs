using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PigWebApplication.Models;

public partial class Pig
{
    [Display(Name = "Номер")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name="Стать [Жіноча - 0, чоловіча - 1]")]
    public short Gender { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Дата народження")]
    public DateTime BirthDate { get; set; }

    public int BreedId { get; set; }
    [Display(Name = "Додаткова інформація")]
    public string? Note { get; set; }
    [Display(Name = "Порода")]
    public virtual Breed Breed { get; set; } = null!;

    public virtual ICollection<Injection> Injections { get; } = new List<Injection>();
}
