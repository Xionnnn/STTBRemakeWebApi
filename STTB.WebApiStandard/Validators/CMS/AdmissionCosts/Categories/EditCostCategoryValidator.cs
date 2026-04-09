using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts.Categories
{
    public class EditCostCategoryValidator : AbstractValidator<EditCostCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public EditCostCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category Name is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditCostCategoryRequest request, ValidationContext<EditCostCategoryRequest> context, CancellationToken ct)
        {
            var category = await _db.AcademicProgramCostCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (category == null)
            {
                context.AddFailure(nameof(EditCostCategoryRequest.Id), "Data doesn't exist");
                return;
            }

            var existingName = await _db.AcademicProgramCostCategories
                .FirstOrDefaultAsync(nc => nc.Id != request.Id && nc.CategoryName.ToUpper() == request.CategoryName.ToUpper(), ct);

            if (existingName != null)
            {
                context.AddFailure(nameof(EditCostCategoryRequest.CategoryName), "The inputted name has already existed");
            }
        }
    }
}
