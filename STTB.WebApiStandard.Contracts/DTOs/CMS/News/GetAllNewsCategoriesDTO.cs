using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.DTOs.CMS.News
{
    public class GetAllNewsCategoriesDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
