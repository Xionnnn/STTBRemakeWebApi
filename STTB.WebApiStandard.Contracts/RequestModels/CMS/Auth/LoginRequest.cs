using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Auth;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Auth
{
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
