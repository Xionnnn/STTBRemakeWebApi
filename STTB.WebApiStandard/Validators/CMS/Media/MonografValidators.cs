using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    public class AddMediaMonografValidator : AbstractValidator<AddMediaMonografRequest>
    {
        public AddMediaMonografValidator()
        {
            RuleFor(x => x.MonografTitle).NotEmpty().WithMessage("Monograf title is required.");
        }
    }

    public class EditMediaMonografValidator : AbstractValidator<EditMediaMonografRequest>
    {
        public EditMediaMonografValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.MonografTitle).NotEmpty().WithMessage("Monograf title is required.");
        }
    }
}
