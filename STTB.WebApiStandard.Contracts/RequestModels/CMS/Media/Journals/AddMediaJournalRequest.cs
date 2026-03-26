using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Journals;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals
{
    public class AddMediaJournalRequest : IRequest<AddMediaJournalResponse>
    {
        public string JournalTitle { get; set; } = string.Empty;
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public string Issn { get; set; } = string.Empty;
        public string EIssn { get; set; } = string.Empty;
        public string Doi { get; set; } = string.Empty;
        public IFormFile? JournalFile { get; set; }
        public IFormFile? Thumbnail { get; set; }
    }
}
