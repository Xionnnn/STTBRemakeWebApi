using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Donations;

namespace STTB.WebApiStandard.Validators.Donations
{
    public class AddDonorMemberValidator : AbstractValidator<AddDonorMemberRequest>
    {
        public AddDonorMemberValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstName is required");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName is required");

            RuleFor(x => x.Salutation)
                .NotEmpty()
                .WithMessage("Salutation is required");

            RuleFor(x => x.Contact)
                .NotEmpty()
                .WithMessage("Contact is required")
                .Matches(@"^\+?[0-9\s\-()]{7,20}$")
                .WithMessage("Contact must be a valid phone number format");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email must be a valid email format");

            RuleFor(x => x.DonationType)
                .NotEmpty()
                .WithMessage("DonationType is required");

            RuleFor(x => x.DonationArea)
                .NotEmpty()
                .WithMessage("DonationArea is required");

            RuleFor(x => x.DonationAmount)
                .GreaterThan(0)
                .WithMessage("DonationAmount must be greater than 0");

            RuleFor(x => x.ProofOfDonationImage)
                .NotNull()
                .WithMessage("ProofOfDonationImage is required");

            RuleFor(x => x.StudentName)
                .NotEmpty()
                .WithMessage("StudentName is required when DonationType is 'beasiswa'")
                .When(x => x.DonationType == "beasiswa");

            RuleFor(x => x.AcademicProgramId)
                .NotNull()
                .WithMessage("AcademicProgramId is required when DonationType is 'beasiswa'")
                .When(x => x.DonationType == "beasiswa");
        }
    }
}
