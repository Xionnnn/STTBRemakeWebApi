using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetVideoCategoriesResponse
    {
        public IReadOnlyList<string> Items { get; set; } = Array.Empty<string>();
    }
}
