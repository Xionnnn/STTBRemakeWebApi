using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;

namespace STTB.WebApiStandard.Validators.CMS.Users.Roles
{
    public class AddRoleValidator : AbstractValidator<AddRoleRequest>
    {
        public AddRoleValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("RoleName is required.");
        }
    }
}
