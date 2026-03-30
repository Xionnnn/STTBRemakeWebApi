using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions
{
    public class DeletePermissionRequest : IRequest<DeletePermissionResponse>
    {
        public long Id { get; set; }
    }
}
