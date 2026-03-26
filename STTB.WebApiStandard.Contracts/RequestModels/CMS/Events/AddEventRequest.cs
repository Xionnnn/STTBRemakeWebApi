using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Events
{
    public class AddEventRequest : IRequest<AddEventResponse>
    {
        public string Slug { get; set; } = string.Empty;
        public string EventTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartsAtDate { get; set; }
        public DateTime? EndsAtDate { get; set; }
        public string OrganizerName { get; set; } = string.Empty;
        public List<string> Category { get; set; } = new List<string>();
        public IFormFile? EventImage { get; set; }
        public bool IsPublished { get; set; }
    }
}
