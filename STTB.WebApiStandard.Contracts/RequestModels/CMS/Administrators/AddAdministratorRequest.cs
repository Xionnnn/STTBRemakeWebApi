using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators
{
    public class AddAdministratorRequest : IRequest<AddAdministratorResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
