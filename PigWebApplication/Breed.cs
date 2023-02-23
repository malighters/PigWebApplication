using System;
using System.Collections.Generic;

namespace PigWebApplication;

public partial class Breed
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Direction { get; set; }

    public virtual ICollection<FeedType> FeedTypes { get; } = new List<FeedType>();

    public virtual ICollection<Pig> Pigs { get; } = new List<Pig>();
}
