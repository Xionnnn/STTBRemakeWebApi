using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories
{
    public class GetNewsCategoryResponse
    {
        public long? Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
