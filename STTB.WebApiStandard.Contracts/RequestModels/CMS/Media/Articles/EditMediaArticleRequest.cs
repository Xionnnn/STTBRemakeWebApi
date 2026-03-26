using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Articles;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles
{
    public class EditMediaArticleRequest : IRequest<EditMediaArticleResponse>
    {
        public long Id { get; set; }
        public string ArticleTitle { get; set; } = string.Empty;
        public string ArticleDescription { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public string ArticleContent { get; set; } = string.Empty;
        public IFormFile? Thumbnail { get; set; }
    }
}
