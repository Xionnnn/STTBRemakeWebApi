using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;

namespace STTB.WebApiStandard.Validators.CMS.Administrators
{
    public class GetAdministratorValidator : AbstractValidator<GetAdministratorRequest>
    {
        public GetAdministratorValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Administrator ID must be greater than 0.");
        }
    }
}
