using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Buletins;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins
{
    public class EditMediaBuletinRequest : IRequest<EditMediaBuletinResponse>
    {
        public long Id { get; set; }
        public string BuletinTitle { get; set; } = string.Empty;
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public string Description { get; set; } = string.Empty;
        public IFormFile? BuletinFile { get; set; }
        public IFormFile? Thumbnail { get; set; }
    }
}
