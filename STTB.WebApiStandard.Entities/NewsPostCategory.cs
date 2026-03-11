using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class NewsPostCategory
{
    public long NewsPostId { get; set; }

    public long CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual NewsCategory Category { get; set; } = null!;

    public virtual NewsPost NewsPost { get; set; } = null!;
}
