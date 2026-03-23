using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News
{
    public class GetAllNewsRequest : IRequest<GetAllNewsResponse>
    {
        public string NewsName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
