using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators
{
    public class EditAdministratorRequest : IRequest<EditAdministratorResponse>
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}