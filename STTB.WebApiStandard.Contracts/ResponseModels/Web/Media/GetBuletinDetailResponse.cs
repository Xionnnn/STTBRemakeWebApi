using STTB.WebApiStandard.Contracts.DTOs.Web.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetBuletinDetailResponse
    {
        public long Id { get; set; }
        public string BuletinTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string BuletinPath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
