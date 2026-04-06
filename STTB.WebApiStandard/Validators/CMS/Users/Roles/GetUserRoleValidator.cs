using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;

namespace STTB.WebApiStandard.Validators.CMS.Users.Roles
{
    public class GetUserRoleValidator : AbstractValidator<GetUserRoleRequest>
    {
        public GetUserRoleValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Role ID must be greater than 0.");
        }
    }
}
