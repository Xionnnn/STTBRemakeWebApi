using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media.Categories
{
    public class GetMediaCategoryValidator : AbstractValidator<GetMediaCategoryRequest>
    {
        private readonly SttbDbContext _db;
        public GetMediaCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetMediaCategoryRequest request, ValidationContext<GetMediaCategoryRequest> context, CancellationToken ct)
        {
            var category = await _db.MediaTopicCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (category == null)
            {
                context.AddFailure(nameof(GetMediaCategoryRequest.Id), "Data doesn't exist");
            }
        }
    }
}
