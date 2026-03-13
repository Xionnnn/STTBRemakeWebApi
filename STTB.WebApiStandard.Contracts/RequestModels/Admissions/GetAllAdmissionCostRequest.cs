using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Admissions;

namespace STTB.WebApiStandard.Contracts.RequestModels.Admissions
{
    public class GetAllAdmissionCostRequest : IRequest<GetAllAdmissionCostResponse>
    {
    }
}
