using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators
{
    public class GetAdministratorResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
