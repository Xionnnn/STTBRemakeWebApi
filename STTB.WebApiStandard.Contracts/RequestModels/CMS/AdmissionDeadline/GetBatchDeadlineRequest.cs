using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionDeadline;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline
{
    public class GetBatchDeadlineRequest : IRequest<GetBatchDeadlineResponse>
    {
        public long Id { get; set; }
    }
}
