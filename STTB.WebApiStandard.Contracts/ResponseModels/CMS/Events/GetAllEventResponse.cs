using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events
{
    public class GetAllEventResponse
    {
        public IReadOnlyList<EventDTO> Items { get; set; } = Array.Empty<EventDTO>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalEvents { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
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
