using System;
using System.Collections.Generic;

namespace practice_hothouse.Models;

public partial class Sowing
{
    public int SowingId { get; set; }

    public int? HectareId { get; set; }

    public int? SeedId { get; set; }

    public DateOnly? SowingDate { get; set; }

    public int? NumberOfPlantedSeeds { get; set; }

    public decimal? ExpectedYield { get; set; }

    public decimal? ActualYield { get; set; }

    public int? IsHarvestClosed { get; set; }

    public int? IsArhive { get; set; }

    public int? NumberInSeason { get; set; }

    public virtual ICollection<HarvestHistory> HarvestHistories { get; set; } = new List<HarvestHistory>();

    public virtual Hectare? Hectare { get; set; }

    public virtual Seed? Seed { get; set; }
}
