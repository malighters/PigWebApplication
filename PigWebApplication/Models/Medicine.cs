using System;
using System.Collections.Generic;

namespace PigWebApplication.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Injection> Injections { get; } = new List<Injection>();
}
