using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class EventCategory
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<EventCategoryMap> EventCategoryMaps { get; set; } = new List<EventCategoryMap>();
}
