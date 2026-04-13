using System;
using System.Collections.Generic;
using STTB.WebApiStandard.Contracts.DTOs.Web.Media;

namespace STTB.WebApiStandard.Contracts.DTOs.Web.Media
{
    public class MediaDTO
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string MediaTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public string MediaDescription { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
