using STTB.WebApiStandard.Contracts.DTOs.CMS.News;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories
{
    public class GetAllNewsCategoriesResponse
    {
        public List<GetAllNewsCategoriesDTO> Items { get; set; } = new List<GetAllNewsCategoriesDTO>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalNews { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
