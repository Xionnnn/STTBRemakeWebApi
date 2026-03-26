using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers
{
    public class EditLecturerRequest : IRequest<EditLecturerResponse>
    {
        public long Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public IFormFile? LecturerImage { get; set; }
        public string OrganizationalRole { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> Degrees { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
