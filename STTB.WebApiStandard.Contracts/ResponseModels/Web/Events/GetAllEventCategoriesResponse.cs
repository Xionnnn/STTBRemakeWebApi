using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Events
{
    public class GetAllEventCategoriesResponse
    {
        public IReadOnlyList<string> Items { get; set; } = Array.Empty<string>();
    }
}
