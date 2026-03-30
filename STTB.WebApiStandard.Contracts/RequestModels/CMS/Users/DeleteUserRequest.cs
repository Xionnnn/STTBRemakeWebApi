using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Users
{
    public class DeleteUserRequest : IRequest<DeleteUserResponse>
    {
        public long Id { get; set; }
    }
}
