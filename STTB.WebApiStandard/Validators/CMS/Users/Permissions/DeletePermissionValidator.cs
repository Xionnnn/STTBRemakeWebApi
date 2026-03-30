using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;

namespace STTB.WebApiStandard.Validators.CMS.Users.Permissions
{
    public class DeletePermissionValidator : AbstractValidator<DeletePermissionRequest>
    {
        public DeletePermissionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Permission ID must be greater than 0.");
        }
    }
}
