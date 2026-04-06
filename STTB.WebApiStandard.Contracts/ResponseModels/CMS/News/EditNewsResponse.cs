using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.News
{
    public class EditNewsResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public List<string> Category { get; set; } = new List<string>();
        public bool IsPublished { get; set; }
    }
}
