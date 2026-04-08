using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories
{
    public class EditMediaCategoryResponse
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
