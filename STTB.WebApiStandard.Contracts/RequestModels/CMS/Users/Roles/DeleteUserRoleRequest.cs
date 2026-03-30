using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles
{
    public class DeleteUserRoleRequest : IRequest<DeleteUserRoleResponse>
    {
        public long Id { get; set; }
    }
}
