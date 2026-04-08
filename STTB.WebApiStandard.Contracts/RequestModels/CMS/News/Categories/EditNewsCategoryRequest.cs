using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories
{
    public class EditNewsCategoryRequest
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
