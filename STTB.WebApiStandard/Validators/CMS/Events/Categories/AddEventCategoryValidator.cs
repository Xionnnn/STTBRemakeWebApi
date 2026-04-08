using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Events.Categories
{
    public class AddEventCategoryValidator : AbstractValidator<AddEventCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public AddEventCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category Name is required.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddEventCategoryRequest request, ValidationContext<AddEventCategoryRequest> context, CancellationToken ct)
        {
            var existing = await _db.EventCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToUpper() == request.CategoryName.ToUpper() || c.Slug == request.Slug, ct);

            if (existing != null)
            {
                context.AddFailure(nameof(AddEventCategoryRequest), "Data Already Exist");
            }
        }
    }
}
