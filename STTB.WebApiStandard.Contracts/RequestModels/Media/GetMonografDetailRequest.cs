using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Media
{
    public class GetMonografDetailRequest
    {
        public string MonografSlug { get; set; } = string.Empty;
    }
}
