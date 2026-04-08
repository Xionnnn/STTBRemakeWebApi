using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.News.Categories
{
    public class AddNewsCategoryValidator : AbstractValidator<AddNewsCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public AddNewsCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .WithMessage("Category Name is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddNewsCategoryRequest request, ValidationContext<AddNewsCategoryRequest> context, CancellationToken ct)
        {
            var existingCategory = await _db.NewsCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToUpper() == request.CategoryName.ToUpper(), ct);

            if (existingCategory != null)
            {
                context.AddFailure(nameof(AddNewsCategoryRequest.CategoryName), "Data Already Exist");
            }
        }
    }
}
