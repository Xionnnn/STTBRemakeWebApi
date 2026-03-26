using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Buletins
{
    public class EditMediaBuletinResponse
    {
        public long Id { get; set; }
        public string BuletinTitle { get; set; } = string.Empty;
        public List<AuthorDTO> Authors { get; set; } = new List<AuthorDTO>();
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public List<string> Category { get; set; } = new List<string>();
        public string BuletinPath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ThumbnailPath { get; set; } = string.Empty;
    }
}
