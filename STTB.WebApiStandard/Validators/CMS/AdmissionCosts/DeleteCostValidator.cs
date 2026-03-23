using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts
{
    public class DeleteCostValidator : AbstractValidator<DeleteCostRequest>
    {
        public DeleteCostValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Cost ID must be greater than 0.");
        }
    }
}
