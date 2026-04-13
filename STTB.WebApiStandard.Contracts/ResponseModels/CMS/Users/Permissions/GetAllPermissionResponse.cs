using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Users.Permissions;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Permissions
{
    public class GetAllPermissionResponse
    {
        public List<PermissionDTO> Items { get; set; } = new List<PermissionDTO>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
