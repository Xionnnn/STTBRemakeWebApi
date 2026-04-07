using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts
{
    public class AddCostValidator : AbstractValidator<AddCostRequest>
    {
        private readonly SttbDbContext _db;

        public AddCostValidator(SttbDbContext db)
        {
            _db = db;
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Category Name is required.");
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage("Program Name is required.");
            RuleFor(x => x.CostName).NotEmpty().WithMessage("Cost Name is required.");
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative.");
            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddCostRequest request, ValidationContext<AddCostRequest> context, CancellationToken ct)
        {
            var existingCost = await _db.AcademicProgramCosts
                .Include(c => c.AcademicProgram)
                .FirstOrDefaultAsync(c => 
                    c.Name.ToUpper() == request.CostName.ToUpper() && 
                    c.AcademicProgram.Name == request.ProgramName, ct);

            if (existingCost != null)
            {
                context.AddFailure(nameof(AddCostRequest), "Data Already Exist");
            }
        }
    }
}
