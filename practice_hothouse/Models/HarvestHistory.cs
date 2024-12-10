using System;
using System.Collections.Generic;

namespace practice_hothouse.Models;

public partial class HarvestHistory
{
    public int HarvestHistoryId { get; set; }

    public int? SowingId { get; set; }

    public int? UserId { get; set; }

    public DateOnly? HarvestDate { get; set; }

    public decimal? HarvestedAmount { get; set; }

    public int? IsArhive { get; set; }

    public virtual Sowing? Sowing { get; set; }

    public virtual User? User { get; set; }
}
