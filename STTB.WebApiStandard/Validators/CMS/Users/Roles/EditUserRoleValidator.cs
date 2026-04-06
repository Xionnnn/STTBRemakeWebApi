using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;

namespace STTB.WebApiStandard.Validators.CMS.Users.Roles
{
    public class EditUserRoleValidator : AbstractValidator<EditUserRoleRequest>
    {
        public EditUserRoleValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Role ID must be greater than 0.");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("RoleName is required.");
        }
    }
}
