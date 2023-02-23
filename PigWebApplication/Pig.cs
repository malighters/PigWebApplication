using System;
using System.Collections.Generic;

namespace PigWebApplication;

public partial class Pig
{
    public int Id { get; set; }

    public short Gender { get; set; }

    public DateTime BirthDate { get; set; }

    public int BreedId { get; set; }

    public string? Note { get; set; }

    public virtual Breed Breed { get; set; } = null!;

    public virtual ICollection<Injection> Injections { get; } = new List<Injection>();
}
