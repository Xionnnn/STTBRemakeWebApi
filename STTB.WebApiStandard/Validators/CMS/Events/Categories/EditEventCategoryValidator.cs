using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Events.Categories
{
    public class EditEventCategoryValidator : AbstractValidator<EditEventCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public EditEventCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category Name is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditEventCategoryRequest request, ValidationContext<EditEventCategoryRequest> context, CancellationToken ct)
        {
            var existing = await _db.EventCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (existing == null)
            {
                context.AddFailure(nameof(EditEventCategoryRequest.Id), "Data doesn't exist");
                return;
            }

            // Exclude current record from uniqueness check
            var duplicate = await _db.EventCategories
                .FirstOrDefaultAsync(c => c.Id != request.Id &&
                    c.Name.ToUpper() == request.CategoryName.ToUpper(), ct);

            if (duplicate != null)
            {
                context.AddFailure(nameof(EditEventCategoryRequest.CategoryName), "The inputted name or slug has already existed");
            }
        }
    }
}
