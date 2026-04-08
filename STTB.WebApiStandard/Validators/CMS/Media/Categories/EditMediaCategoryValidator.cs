using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media.Categories
{
    public class EditMediaCategoryValidator : AbstractValidator<EditMediaCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public EditMediaCategoryValidator(SttbDbContext db)
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

        private async Task ValidateBusinessAsync(EditMediaCategoryRequest request, ValidationContext<EditMediaCategoryRequest> context, CancellationToken ct)
        {
            var category = await _db.MediaTopicCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (category == null)
            {
                context.AddFailure(nameof(EditMediaCategoryRequest.Id), "Data doesn't exist");
                return;
            }

            var existingName = await _db.MediaTopicCategories
                .FirstOrDefaultAsync(nc => nc.Id != request.Id && nc.Name.ToUpper() == request.CategoryName.ToUpper(), ct);

            if (existingName != null)
            {
                context.AddFailure(nameof(EditMediaCategoryRequest.CategoryName), "The inputted name has already existed");
            }
        }
    }
}
