using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Dashboards;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Dashboard
{
    public class GetDashboardDataRequest : IRequest<GetDashboardDataResponse>
    {
    }
}
