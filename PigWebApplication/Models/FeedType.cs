using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PigWebApplication.Models;

public partial class FeedType
{
    public int Id { get; set; }

    public int BreedId { get; set; }

    public int FeedmixId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "День початку годування")]
    public short AgeStart { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "День завершення годування")]
    public short AgeFinish { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кількість їжі на день")]
    public float QuantityPerBig { get; set; }
    [Display(Name = "Порода")]
    public virtual Breed Breed { get; set; } = null!;
    [Display(Name = "Харчова суміш")]
    public virtual FeedMixture Feedmix { get; set; } = null!;
}
