using System;
using System.Collections.Generic;

namespace PigWebApplication;

public partial class FeedType
{
    public int Id { get; set; }

    public int BreedId { get; set; }

    public int FeedmixId { get; set; }

    public short AgeStart { get; set; }

    public short AgeFinish { get; set; }

    public float QuantityPerBig { get; set; }

    public virtual Breed Breed { get; set; } = null!;

    public virtual FeedMixture Feedmix { get; set; } = null!;
}
