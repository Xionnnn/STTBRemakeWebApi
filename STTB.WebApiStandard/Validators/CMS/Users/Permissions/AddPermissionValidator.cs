using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;

namespace STTB.WebApiStandard.Validators.CMS.Users.Permissions
{
    public class AddPermissionValidator : AbstractValidator<AddPermissionRequest>
    {
        public AddPermissionValidator()
        {
            RuleFor(x => x.PermissionName)
                .NotEmpty().WithMessage("PermissionName is required.");
        }
    }
}
