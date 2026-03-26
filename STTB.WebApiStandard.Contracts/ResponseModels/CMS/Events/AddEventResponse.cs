using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events
{
    public class AddEventResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string EventTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartsAtDate { get; set; }
        public DateTime? EndsAtDate { get; set; }
        public string OrganizerName { get; set; } = string.Empty;
        public List<string> Category { get; set; } = new List<string>();
        public string ImagePath { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
    }
}
