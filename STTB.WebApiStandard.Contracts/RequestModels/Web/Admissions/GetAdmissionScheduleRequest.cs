using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Admissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Admissions
{
    public class GetAdmissionScheduleRequest : IRequest<GetAdmissionScheduleResponse>
    {
    }
}
