using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.News
{
    public class GetAllNewsResponse
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
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublicationDate { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public IReadOnlyList<string> Category { get; set; } = Array.Empty<string>();
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
