using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Lecturers;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers
{
    public class GetAllLecturerRequest : IRequest<GetAllLecturerResponse>
    {
        public string LecturerName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
