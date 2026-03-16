using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.Libraries;

namespace STTB.WebApiStandard.Contracts.RequestModels.Libraries
{
    public class AddLibraryMemberRequest : IRequest<AddLibraryMemberResponse>
    {
        public string FullName { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public string InstitutionName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile? PassportImage { get; set; } = null;
        public IFormFile? IdImage { get; set; } = null;
        public IFormFile? ProofOfDepositImage { get; set; } = null;
    }
}
