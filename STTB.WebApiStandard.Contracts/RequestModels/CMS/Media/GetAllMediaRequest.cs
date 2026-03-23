using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media
{
    public class GetAllMediaRequest : IRequest<GetAllMediaResponse>
    {
        public string MediaName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
