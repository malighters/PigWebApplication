using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PigWebApplication.Models;

public partial class Injection
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Час проставлення ліків")]
    public DateTime Date { get; set; }

    public int MedicineId { get; set; }
    [Display(Name = "Номер свині")]
    public int PigId { get; set; }
    [Display(Name = "Нотатки")]
    public string? Note { get; set; }
    [Display(Name = "Назва препарату")]
    public virtual Medicine Medicine { get; set; } = null!;
    [Display(Name = "Номер свині")]
    public virtual Pig Pig { get; set; } = null!;
}
