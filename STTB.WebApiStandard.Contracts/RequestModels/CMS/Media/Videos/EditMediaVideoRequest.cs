using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Videos;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos
{
    public class EditMediaVideoRequest : IRequest<EditMediaVideoResponse>
    {
        public long Id { get; set; }
        public string VideoTitle { get; set; } = string.Empty;
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public string VideoDescription { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public string VideoUrl { get; set; } = string.Empty;
        public IFormFile? Thumbnail { get; set; }
    }
}
