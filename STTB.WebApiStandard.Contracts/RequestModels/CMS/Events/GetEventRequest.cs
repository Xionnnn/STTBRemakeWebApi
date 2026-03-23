using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Events
{
    public class GetEventRequest : IRequest<GetEventResponse>
    {
        public long Id { get; set; }
    }
}
