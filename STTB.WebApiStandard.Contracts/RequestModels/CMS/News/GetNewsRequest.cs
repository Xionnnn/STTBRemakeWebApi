using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News
{
    public class GetNewsRequest : IRequest<GetNewsResponse>
    {
        public long Id { get; set; }
    }
}
