using System;
using System.Collections.Generic;

namespace JobPortal.Database.AppDbContextModels;

public partial class JobApplication
{
    public Guid Id { get; set; }

    public Guid JobId { get; set; }

    public Guid UserId { get; set; }

    public string? Status { get; set; }

    public DateTime? AppliedAt { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
