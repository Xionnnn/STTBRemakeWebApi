using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Journals
{
    public class EditMediaJournalResponse
    {
        public long Id { get; set; }
        public string JournalTitle { get; set; } = string.Empty;
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public string Abstract { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public string JournalPath { get; set; } = string.Empty;
        public string Issn { get; set; } = string.Empty;
        public string EIssn { get; set; } = string.Empty;
        public string Doi { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
