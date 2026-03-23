using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Events
{
    public class EditEventRequest : IRequest<EditEventResponse>
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
        public IFormFile? EventImage { get; set; }
        public bool IsPublished { get; set; }
    }
}
