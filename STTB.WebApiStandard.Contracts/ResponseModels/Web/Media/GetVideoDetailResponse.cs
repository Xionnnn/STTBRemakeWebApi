using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetVideoDetailResponse
    {
        public long Id { get; set; }
        public string VideoTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public string VideoDescription { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string VideoUrl { get; set; } = string.Empty;
    }
}
