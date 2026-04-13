using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Users.Roles;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users.Roles
{
    public class GetAllUserRoleResponse
    {
        public List<RoleDTO> Items { get; set; } = new List<RoleDTO>();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
