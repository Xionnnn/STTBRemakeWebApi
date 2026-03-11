using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Events
{
    public class GetAvailableEventsResponse
    {
        public IReadOnlyList<EventDTO> Items { get; set; } = Array.Empty<EventDTO>();
    }

    public class EventDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string EventDate { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }

}
