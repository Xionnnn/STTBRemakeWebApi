using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Academic;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Academics
{
    public class GetAvailableProgramRequest : IRequest<GetAvailableProgramResponse>
    {
        public int? FetchLimit { get; set; } = null;
    }
}
