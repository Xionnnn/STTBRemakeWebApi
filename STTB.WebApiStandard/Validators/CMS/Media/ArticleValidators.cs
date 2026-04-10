using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media.Articles
{
    public class AddMediaArticleValidator : AbstractValidator<AddMediaArticleRequest>
    {
        public AddMediaArticleValidator()
        {
            RuleFor(x => x.ArticleTitle).NotEmpty().WithMessage("Article title is required.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");
        }
    }

    public class EditMediaArticleValidator : AbstractValidator<EditMediaArticleRequest>
    {
        private readonly SttbDbContext _db;

        public EditMediaArticleValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.ArticleTitle).NotEmpty().WithMessage("Article title is required.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditMediaArticleRequest request, ValidationContext<EditMediaArticleRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "article", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(EditMediaArticleRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class GetMediaArticleValidator : AbstractValidator<GetMediaArticleRequest>
    {
        private readonly SttbDbContext _db;

        public GetMediaArticleValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetMediaArticleRequest request, ValidationContext<GetMediaArticleRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "article", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(GetMediaArticleRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class DeleteMediaArticleValidator : AbstractValidator<DeleteMediaArticleRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaArticleValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(DeleteMediaArticleRequest request, ValidationContext<DeleteMediaArticleRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "article", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(DeleteMediaArticleRequest.Id), "Data doesn't exist");
            }
        }
    }
}
