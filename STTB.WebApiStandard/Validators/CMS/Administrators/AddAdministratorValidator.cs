using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;

namespace STTB.WebApiStandard.Validators.CMS.Administrators
{
    public class AddAdministratorValidator : AbstractValidator<AddAdministratorRequest>
    {
        public AddAdministratorValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Administrator Name is required.");
        }
    }
}
