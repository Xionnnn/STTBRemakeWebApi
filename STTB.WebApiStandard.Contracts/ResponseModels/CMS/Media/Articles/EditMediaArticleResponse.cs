using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Articles
{
    public class EditMediaArticleResponse
    {
        public long Id { get; set; }
        public string ArticleTitle { get; set; } = string.Empty;
        public string ArticleDescription { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public string ArticleContent { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
