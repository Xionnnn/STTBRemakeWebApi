using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.News
{
    public class AddNewsValidator : AbstractValidator<AddNewsRequest>
    {
        private readonly SttbDbContext _db;
        public AddNewsValidator(SttbDbContext db)
        {
            _db = db;
            RuleFor(x => x.Title).NotEmpty().WithMessage("News Title is required.");
            RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required.");
            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }
        private async Task ValidateBusinessAsync(AddNewsRequest request, ValidationContext<AddNewsRequest> context, CancellationToken ct)
        {
            var existingNews = await _db.NewsPosts
                .FirstOrDefaultAsync(np => np.Slug == request.Slug || np.Title.ToUpper() == request.Title.ToUpper());

            if (existingNews != null)
            {
                context.AddFailure(nameof(AddNewsRequest), "Data Already Exist");
            }
        }
    }
}
