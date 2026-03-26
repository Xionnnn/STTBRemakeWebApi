using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Monograf
{
    public class EditMediaMonografResponse
    {
        public long Id { get; set; }
        public string MonografTitle { get; set; } = string.Empty;
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public string Synopsis { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
