using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Events
{
    public class EventDTO
    {
        public string EventTitle { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime StartsAtDate { get; set; }
        public DateTime EndsAtDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string ImagePath { get; set; } = string.Empty;
    }
}
