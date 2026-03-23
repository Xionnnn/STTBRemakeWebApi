using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators
{
    public class DeleteAdministratorRequest : IRequest<DeleteAdministratorResponse>
    {
        public long Id { get; set; }
    }
}
