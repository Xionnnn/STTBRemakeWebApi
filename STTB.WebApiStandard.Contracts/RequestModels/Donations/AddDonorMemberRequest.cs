using MediatR;
using Microsoft.AspNetCore.Http;
using STTB.WebApiStandard.Contracts.ResponseModels.Donations;

namespace STTB.WebApiStandard.Contracts.RequestModels.Donations
{
    public class AddDonorMemberRequest : IRequest<AddDonorMemberResponse>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Salutation { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DonationType { get; set; } = string.Empty;
        public string DonationArea { get; set; } = string.Empty;
        public bool ProofOfSupport { get; set; }
        public IFormFile? ProofOfDonationImage { get; set; }
        public decimal DonationAmount { get; set; }
        public string? StudentName { get; set; }
        public long? AcademicProgramId { get; set; }
    }
}
