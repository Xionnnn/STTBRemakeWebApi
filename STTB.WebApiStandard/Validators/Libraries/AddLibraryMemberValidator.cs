using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Libraries;

namespace STTB.WebApiStandard.Validators.Libraries
{
    public class AddLibraryMemberValidator : AbstractValidator<AddLibraryMemberRequest>
    {
        public AddLibraryMemberValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("FullName is required");

            RuleFor(x => x.DOB)
                .NotEmpty()
                .WithMessage("DOB is required");

            RuleFor(x => x.InstitutionName)
                .NotEmpty()
                .WithMessage("InstitutionName is required");

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

            RuleFor(x => x.PassportImage)
                .NotNull()
                .WithMessage("PassportImage is required");

            RuleFor(x => x.IdImage)
                .NotNull()
                .WithMessage("IdImage is required");

            RuleFor(x => x.ProofOfDepositImage)
                .NotNull()
                .WithMessage("ProofOfDepositImage is required");
        }
    }
}
