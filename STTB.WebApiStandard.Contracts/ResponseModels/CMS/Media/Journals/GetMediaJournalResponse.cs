using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Journals
{
    public class GetMediaJournalResponse
    {
        public long Id { get; set; }
        public string JournalTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string JournalPath { get; set; } = string.Empty;
        public string Issn { get; set; } = string.Empty;
        public string EIssn { get; set; } = string.Empty;
        public string Doi { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
