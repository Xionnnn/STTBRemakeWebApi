using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.CMS.Users;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Users
{
    public class GetAllUserResponse
    {
        public List<CMSUserDTO> Items { get; set; } = new List<CMSUserDTO>();
    }
}
