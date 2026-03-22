using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.profiles
{
    public class GetAllLecturerResponse
    {
        public IReadOnlyList<LecturerDto> Items { get; set; } = Array.Empty<LecturerDto>();
    }
    public class LecturerDto
    {
        public long Id { get; set; }
        public string LecturerName { get; set; } = string.Empty;
        public string OrganizationalRole { get; set; } = string.Empty;
        public string LecturerImagePath { get; set; } = string.Empty;
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Degrees { get; set; } = Array.Empty<string>();
    }
}
