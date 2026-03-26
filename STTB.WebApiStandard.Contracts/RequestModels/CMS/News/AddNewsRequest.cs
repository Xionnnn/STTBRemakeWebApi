using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News
{
    public class AddNewsRequest : IRequest<AddNewsResponse>
    {
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IFormFile? NewsImage { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public bool IsPublished { get; set; }
    }
}
