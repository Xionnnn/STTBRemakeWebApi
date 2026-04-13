using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.Media;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetArticleDetailResponse
    {
        public long Id { get; set; }
        public string ArticleTitle { get; set; } = string.Empty;
        public string ArticleDescription { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public string ArticleContent { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }   
}
