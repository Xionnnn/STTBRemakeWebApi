using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionCosts.Categories
{
    public class GetCostCategoryValidator : AbstractValidator<GetCostCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public GetCostCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetCostCategoryRequest request, ValidationContext<GetCostCategoryRequest> context, CancellationToken ct)
        {
            var category = await _db.AcademicProgramCostCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (category == null)
            {
                context.AddFailure(nameof(GetCostCategoryRequest.Id), "Data doesn't exist");
            }
        }
    }
}
