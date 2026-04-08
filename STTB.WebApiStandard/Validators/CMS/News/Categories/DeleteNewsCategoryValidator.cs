using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.News.Categories
{
    public class DeleteNewsCategoryValidator : AbstractValidator<DeleteNewsCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteNewsCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(DeleteNewsCategoryRequest request, ValidationContext<DeleteNewsCategoryRequest> context, CancellationToken ct)
        {
            var newsCategory = await _db.NewsCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (newsCategory == null)
            {
                context.AddFailure(nameof(DeleteNewsCategoryRequest.Id), "Data doesn't exist");
            }
        }
    }
}
