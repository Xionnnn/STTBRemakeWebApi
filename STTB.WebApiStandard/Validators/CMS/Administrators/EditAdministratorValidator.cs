using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;

namespace STTB.WebApiStandard.Validators.CMS.Administrators
{
    public class EditAdministratorValidator : AbstractValidator<EditAdministratorRequest>
    {
        public EditAdministratorValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Administrator ID must be greater than 0.");
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(x => x.Division)
                .NotEmpty().WithMessage("Division is required.")
                .MaximumLength(255).WithMessage("Division cannot exceed 255 characters.");

            RuleFor(x => x.Role)
                .MaximumLength(255).When(x => !string.IsNullOrEmpty(x.Role))
                .WithMessage("Role cannot exceed 255 characters.");
        }
    }
}
