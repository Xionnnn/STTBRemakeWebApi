using System;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.Media
{
    public class MediaItemDTO
    {
        public long Id { get; set; }
        public string MediaName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string MediaFormat { get; set; } = string.Empty;
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
