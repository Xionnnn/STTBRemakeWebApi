using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.News
{
    public class GetAllNewsCategoriesResponse
    {
        public IReadOnlyList<string> Items { get; set; } = Array.Empty<string>();
    }
}
