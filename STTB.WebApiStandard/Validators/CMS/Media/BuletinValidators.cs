using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    public class AddMediaBuletinValidator : AbstractValidator<AddMediaBuletinRequest>
    {
        public AddMediaBuletinValidator()
        {
            RuleFor(x => x.BuletinTitle).NotEmpty().WithMessage("Buletin title is required.");
        }
    }

    public class EditMediaBuletinValidator : AbstractValidator<EditMediaBuletinRequest>
    {
        public EditMediaBuletinValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.BuletinTitle).NotEmpty().WithMessage("Buletin title is required.");
        }
    }
}
