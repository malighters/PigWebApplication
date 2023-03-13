using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PigWebApplication.Models;

public partial class Medicine
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва ліків")]
    public string Name { get; set; } = null!;
    [Display(Name = "Опис")]
    public string? Description { get; set; }

    public virtual ICollection<Injection> Injections { get; } = new List<Injection>();
}
