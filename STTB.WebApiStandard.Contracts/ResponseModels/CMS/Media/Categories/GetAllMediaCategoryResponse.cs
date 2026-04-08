using STTB.WebApiStandard.Contracts.DTOs.CMS.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories
{
    public class GetAllMediaCategoryResponse
    {
        public List<MediaCategoryDTO> Categories { get; set; } = new List<MediaCategoryDTO>();

        //pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalMedia { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
