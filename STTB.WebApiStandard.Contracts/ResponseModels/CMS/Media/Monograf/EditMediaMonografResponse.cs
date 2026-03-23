using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Monograf
{
    public class EditMediaMonografResponse
    {
        public long Id { get; set; }
        public string MonografTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string Synopsis { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
