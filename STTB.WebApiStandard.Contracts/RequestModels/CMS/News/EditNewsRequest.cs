using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News
{
    public class EditNewsRequest : IRequest<EditNewsResponse>
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public IFormFile? NewsImage { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public bool IsPublished { get; set; }
    }
}
