using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events.Categories;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Events.Categories
{
    public class GetEventCategoryValidator : AbstractValidator<GetEventCategoryRequest>
    {
        private readonly SttbDbContext _db;

        public GetEventCategoryValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetEventCategoryRequest request, ValidationContext<GetEventCategoryRequest> context, CancellationToken ct)
        {
            var existing = await _db.EventCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (existing == null)
            {
                context.AddFailure(nameof(GetEventCategoryRequest.Id), "Data doesn't exist");
            }
        }
    }
}
