using System;
using System.Collections.Generic;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media
{
    public class GetAllMediaResponse
    {
        public IReadOnlyList<MediaItemDTO> Items { get; set; } = Array.Empty<MediaItemDTO>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMedia { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
