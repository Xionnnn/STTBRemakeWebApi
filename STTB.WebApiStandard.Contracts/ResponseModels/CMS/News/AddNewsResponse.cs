using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.News
{
    public class AddNewsResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public bool IsPublished { get; set; }
    }
}
