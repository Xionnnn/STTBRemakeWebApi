using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Monograf;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf
{
    public class AddMediaMonografRequest : IRequest<AddMediaMonografResponse>
    {
        public string MonografTitle { get; set; } = string.Empty;
        public IReadOnlyList<AuthorDTO> Authors { get; set; } = Array.Empty<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public string Synopsis { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public IFormFile? Thumbnail { get; set; }
    }
}
