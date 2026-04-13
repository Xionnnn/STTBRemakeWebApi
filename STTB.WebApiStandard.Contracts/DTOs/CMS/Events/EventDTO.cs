using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Events
{
    public class EventDTO
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string EventTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartsAtDate { get; set; }
        public DateTime? EndsAtDate { get; set; }
        public string OrganizerName { get; set; } = string.Empty;
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string ImagePath { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public DateTime CreatedAt {  get; set; }
    }
}
