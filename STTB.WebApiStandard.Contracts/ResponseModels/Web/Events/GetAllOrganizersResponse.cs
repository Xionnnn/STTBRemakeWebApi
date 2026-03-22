using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Events
{
    public class GetAllOrganizersResponse
    {
        public IReadOnlyList<string> Items { get; set; } = Array.Empty<string>();
    }
}

