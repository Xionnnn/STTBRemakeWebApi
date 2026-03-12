using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetJournalDetailResponse
    {
        public long Id { get; set; }
        public string JournalTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string JournalDescription { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string JournalPath { get; set; } = string.Empty;
    }
}
