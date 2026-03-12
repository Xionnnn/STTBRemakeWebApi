using STTB.WebApiStandard.Contracts.ResponseModels.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetAvailableMediaResponse
    {
        public IReadOnlyList<MediaDTO> Items { get; set; } = Array.Empty<MediaDTO>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMedia { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
    public class MediaDTO
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string MediaTitle { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string MediaDescription { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
    