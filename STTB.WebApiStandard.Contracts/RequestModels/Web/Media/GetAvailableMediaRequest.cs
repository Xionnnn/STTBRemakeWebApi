using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Media
{
    public class GetAvailableMediaRequest : IRequest<GetAvailableMediaResponse>
    {
        public string MediaFormat { get; set; } = string.Empty;
        public string MediaTitle { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public DateTime? PublicationDate { get; set; }
        public int? FetchLimit { get; set; } = null;
    }
}
