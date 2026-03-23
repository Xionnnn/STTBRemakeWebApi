using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts
{
    public class EditCostValidator : AbstractValidator<EditCostRequest>
    {
        public EditCostValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Cost ID must be greater than 0.");
            
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("CategoryName is required.");
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage("ProgramName is required.");
            RuleFor(x => x.CostName).NotEmpty().WithMessage("CostName is required.");
            
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage("Cost must be 0 or greater.");
        }
    }
}
