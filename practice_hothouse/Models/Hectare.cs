using System;
using System.Collections.Generic;

namespace practice_hothouse.Models;

public partial class Hectare
{
    public int HectareId { get; set; }

    public int? PlotId { get; set; }

    public string? HectareName { get; set; }

    public string? Description { get; set; }

    public virtual Plot? Plot { get; set; }

    public virtual ICollection<Sowing> Sowings { get; set; } = new List<Sowing>();
}
