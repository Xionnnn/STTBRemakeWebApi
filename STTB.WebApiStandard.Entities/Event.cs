using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class Event
{
    public long Id { get; set; }

    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Summary { get; set; }

    public string? Description { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public long? EventOrganizerId { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<EventCategoryMap> EventCategoryMaps { get; set; } = new List<EventCategoryMap>();

    public virtual EventOrganizer? EventOrganizer { get; set; }
}
