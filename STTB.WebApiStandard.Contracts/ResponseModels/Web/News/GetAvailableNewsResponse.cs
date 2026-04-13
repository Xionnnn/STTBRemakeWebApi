using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.Web.News;

namespace STTB.WebApiStandard.Contracts.ResponseModels.News
{
    public class GetAvailableNewsResponse
    {
        public IReadOnlyList<NewsDTO> Items { get; set; } = Array.Empty<NewsDTO>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNews { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
