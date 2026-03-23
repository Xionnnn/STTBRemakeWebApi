using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Administrators
{
    public class GetAllAdministratorResponse
    {
        public IReadOnlyList<AdministratorDTO> Items { get; set; } = Array.Empty<AdministratorDTO>();

        //Pagination metadata
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
    public class AdministratorDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

    }
}
