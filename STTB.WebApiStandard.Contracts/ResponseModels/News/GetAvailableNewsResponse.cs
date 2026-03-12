using System;
using System.Collections.Generic;
using System.Text;

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
    public class NewsDTO
    {
        public int? Id { get; set; } = null;
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
