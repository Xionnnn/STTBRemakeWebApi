using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Media
{
    public class GetBuletinDetailRequest : IRequest<GetBuletinDetailResponse>
    {
        public string BuletinSlug { get; set; } = string.Empty;
    }
}
