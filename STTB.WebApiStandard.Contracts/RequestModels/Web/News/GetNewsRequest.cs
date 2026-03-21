using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.News
{
    public class GetNewsRequest : IRequest<GetNewsResponse>
    {
        public string NewsSlug { get; set; } = string.Empty;
    }
}
