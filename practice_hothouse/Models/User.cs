using System;
using System.Collections.Generic;

namespace practice_hothouse.Models;

public partial class User
{
    public int UserId { get; set; }

    public int? PlotId { get; set; }

    public string? UserFname { get; set; }

    public string? UserLname { get; set; }

    public string? UserSname { get; set; }

    public string? JobTitle { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    public int? IsArhive { get; set; }

    public virtual ICollection<HarvestHistory> HarvestHistories { get; set; } = new List<HarvestHistory>();

    public virtual Plot? Plot { get; set; }
}
