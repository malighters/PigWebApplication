using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PigWebApplication.Models;

public partial class Breed
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва породи")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Направлення")]
    public string? Direction { get; set; }

    public virtual ICollection<FeedType> FeedTypes { get; } = new List<FeedType>();

    public virtual ICollection<Pig> Pigs { get; } = new List<Pig>();
}
