using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media
{
    public class GetAllMediaResponse
    {
        public IReadOnlyList<MediaItemDTO> Items { get; set; } = Array.Empty<MediaItemDTO>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMedia { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class MediaItemDTO
    {
        public long Id { get; set; }
        public string MediaName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string MediaFormat { get; set; } = string.Empty;
        public DateTime? PublishedAt { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
