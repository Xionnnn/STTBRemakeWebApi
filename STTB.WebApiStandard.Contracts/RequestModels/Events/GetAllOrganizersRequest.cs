using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Events
{
    public class GetAllOrganizersRequest : IRequest<GetAllOrganizersResponse>
    {
    }
}
