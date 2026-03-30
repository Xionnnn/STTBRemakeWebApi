using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionDeadline;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline
{
    public class GetAllBatchDeadlineRequest : IRequest<GetAllBatchDeadlineResponse>
    {
        public string SearchBatch { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
