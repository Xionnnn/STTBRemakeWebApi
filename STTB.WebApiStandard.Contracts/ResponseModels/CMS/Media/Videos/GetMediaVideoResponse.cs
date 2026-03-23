using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Videos
{
    public class GetMediaVideoResponse
    {
        public long Id { get; set; }
        public string VideoTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public string VideoDescription { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string VideoUrl { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
