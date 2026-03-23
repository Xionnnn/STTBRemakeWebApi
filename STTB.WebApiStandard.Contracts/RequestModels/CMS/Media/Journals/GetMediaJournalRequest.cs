using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Journals;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals
{
    public class GetMediaJournalRequest : IRequest<GetMediaJournalResponse>
    {
        public long Id { get; set; }
    }
}
