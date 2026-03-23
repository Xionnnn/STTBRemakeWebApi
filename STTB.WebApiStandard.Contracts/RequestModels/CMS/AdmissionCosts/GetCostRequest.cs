using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts
{
    public class GetCostRequest : IRequest<GetCostResponse>
    {
        public long Id { get; set; }
    }
}
