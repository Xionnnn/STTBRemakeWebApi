using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts.Categories
{
    public class AddCostCategoryValidator : AbstractValidator<AddCostCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public AddCostCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category Name is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddCostCategoryRequest request, ValidationContext<AddCostCategoryRequest> context, CancellationToken ct)
        {
            var existingCategory = await _db.AcademicProgramCostCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryName.ToUpper() == request.CategoryName.ToUpper(), ct);

            if (existingCategory != null)
            {
                context.AddFailure(nameof(AddCostCategoryRequest.CategoryName), "Data Already Exist");
            }
        }
    }
}
