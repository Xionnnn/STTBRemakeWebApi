using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Events
{
    public class GetEventResponse
    {
        public string EventName { get; set; } = string.Empty;
        public DateTime StartAtDate { get; set; }
        public DateTime? EndsAtDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }

}
