using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class EventOrganizer
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string OrganizerType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
