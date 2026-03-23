using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Auth
{
    public class RegisterRequest : IRequest<RegisterResponse>
    {
        public string FullName { get; set; } = null!;
        public string RoleName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
