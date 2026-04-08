using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media.Categories
{
    public class AddMediaCategoryValidator : AbstractValidator<AddMediaCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public AddMediaCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category Name is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddMediaCategoryRequest request, ValidationContext<AddMediaCategoryRequest> context, CancellationToken ct)
        {
            var existingCategory = await _db.MediaTopicCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToUpper() == request.CategoryName.ToUpper(), ct);

            if (existingCategory != null)
            {
                context.AddFailure(nameof(AddMediaCategoryRequest.CategoryName), "Data Already Exist");
            }
        }
    }
}
