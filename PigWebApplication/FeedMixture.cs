using System;
using System.Collections.Generic;

namespace PigWebApplication;

public partial class FeedMixture
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public short Wheet { get; set; }

    public short Barley { get; set; }

    public short Corn { get; set; }

    public short Pea { get; set; }

    public short Oilcake { get; set; }

    public virtual ICollection<FeedType> FeedTypes { get; } = new List<FeedType>();
}
