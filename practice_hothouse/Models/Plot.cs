using System;
using System.Collections.Generic;

namespace practice_hothouse.Models;

public partial class Plot
{
    public int PlotId { get; set; }

    public string? PlotName { get; set; }

    public string? Cover { get; set; }

    public virtual ICollection<Hectare> Hectares { get; set; } = new List<Hectare>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
