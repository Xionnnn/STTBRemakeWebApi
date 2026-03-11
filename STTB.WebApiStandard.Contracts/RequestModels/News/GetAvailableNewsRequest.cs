using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.News
{
    public class GetAvailableNewsRequest : IRequest<GetAvailableNewsResponse>
    {
        public string NewsTitle { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public DateTime? EventDate { get; set; }
        public int? FetchLimit { get; set; } = null;
    }
}
