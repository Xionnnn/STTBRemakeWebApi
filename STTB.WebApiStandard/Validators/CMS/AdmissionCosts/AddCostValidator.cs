using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts
{
    public class AddCostValidator : AbstractValidator<AddCostRequest>
    {
        public AddCostValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name is required.");
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage("Program Name is required.");
            RuleFor(x => x.CostName).NotEmpty().WithMessage("Cost Name is required.");
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative.");
        }
    }
}
