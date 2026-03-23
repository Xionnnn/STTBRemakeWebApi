using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers
{
    public class AddLecturerRequest : IRequest<AddLecturerResponse>
    {
        public string LecturerName { get; set; } = string.Empty;
        public IFormFile? LecturerImage { get; set; }
        public string OrganizationalRole { get; set; } = string.Empty;
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Degrees { get; set; } = Array.Empty<string>();
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
