using System;
using System.Collections.Generic;

namespace JobPortal.Database.AppDbContextModels;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public bool? IsRevoked { get; set; }

    public virtual User User { get; set; } = null!;
}
