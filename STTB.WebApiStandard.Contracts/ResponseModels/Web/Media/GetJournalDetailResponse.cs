using STTB.WebApiStandard.Contracts.DTOs.Web.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetJournalDetailResponse
    {
        public long Id { get; set; }
        public string JournalTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public string Abstract { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string JournalPath { get; set; } = string.Empty;
        public string Issn { get; set; } = string.Empty;
        public string EIssn { get; set; } = string.Empty;
        public string Doi { get; set; } = string.Empty;
    }
}
