using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.Events;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Events
{
    public class GetAvailableEventResponse
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
}
