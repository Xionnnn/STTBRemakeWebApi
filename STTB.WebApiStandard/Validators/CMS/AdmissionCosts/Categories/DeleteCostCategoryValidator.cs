using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts.Categories
{
    public class DeleteCostCategoryValidator : AbstractValidator<DeleteCostCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteCostCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(DeleteCostCategoryRequest request, ValidationContext<DeleteCostCategoryRequest> context, CancellationToken ct)
        {
            var category = await _db.AcademicProgramCostCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (category == null)
            {
                context.AddFailure(nameof(DeleteCostCategoryRequest.Id), "Data doesn't exist");
            }
        }
    }
}
