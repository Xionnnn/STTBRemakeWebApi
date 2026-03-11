using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class EventCategoryMap
{
    public long EventId { get; set; }

    public long CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual EventCategory Category { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
