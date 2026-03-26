using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media
{
    public class GetAllMediaCategoryResponse
    {
        public IReadOnlyList<string> Categories { get; set; } = Array.Empty<string>();
    }
}
