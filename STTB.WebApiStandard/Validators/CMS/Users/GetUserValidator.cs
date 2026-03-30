using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users;

namespace STTB.WebApiStandard.Validators.CMS.Users
{
    public class GetUserValidator : AbstractValidator<GetUserRequest>
    {
        public GetUserValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");
        }
    }
}
