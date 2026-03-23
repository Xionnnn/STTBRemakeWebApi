using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts
{
    public class DeleteCostRequest : IRequest<DeleteCostResponse>
    {
        public long Id { get; set; }
    }
}
