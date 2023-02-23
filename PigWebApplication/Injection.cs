using System;
using System.Collections.Generic;

namespace PigWebApplication;

public partial class Injection
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int MedicineId { get; set; }

    public int PigId { get; set; }

    public string? Note { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual Pig Pig { get; set; } = null!;
}
