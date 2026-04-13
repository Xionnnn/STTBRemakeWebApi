using STTB.WebApiStandard.Contracts.ResponseModels.Events;
using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.Media;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Media
{
    public class GetAvailableMediaResponse
    {
        public IReadOnlyList<MediaDTO> Items { get; set; } = Array.Empty<MediaDTO>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMedia { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
    