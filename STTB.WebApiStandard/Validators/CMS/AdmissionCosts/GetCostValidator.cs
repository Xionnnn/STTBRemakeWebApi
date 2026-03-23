using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts
{
    public class GetCostValidator : AbstractValidator<GetCostRequest>
    {
        public GetCostValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Cost ID must be greater than 0.");
        }
    }
}
