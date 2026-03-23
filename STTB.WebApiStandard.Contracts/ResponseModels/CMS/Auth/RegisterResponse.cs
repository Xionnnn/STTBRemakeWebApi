using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.Auth
{
    public class RegisterResponse
    {
        public string FullName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsSuccess { get; set; }
    }
}
