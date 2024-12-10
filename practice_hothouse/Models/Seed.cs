using System;
using System.Collections.Generic;

namespace practice_hothouse.Models;

public partial class Seed
{
    public int SeedId { get; set; }

    public string? SeedType { get; set; }

    public string? SeedVariety { get; set; }

    public string? SeedPlantingMethod { get; set; }

    public int? DaysToGermination { get; set; }

    public string? AdditionalNotes { get; set; }

    public int? DaysToFirstHarvest { get; set; }

    public int? HarvestFrequency { get; set; }

    public int? NumberOfHarvests { get; set; }

    public virtual ICollection<Sowing> Sowings { get; set; } = new List<Sowing>();
}
