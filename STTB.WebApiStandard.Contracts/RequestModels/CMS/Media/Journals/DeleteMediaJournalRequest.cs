using MediatR;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals
{
    public class DeleteMediaJournalRequest : IRequest
    {
        public long Id { get; set; }
    }
}
