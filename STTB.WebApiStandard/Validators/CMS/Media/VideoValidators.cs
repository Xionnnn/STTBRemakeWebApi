using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    public class AddMediaVideoValidator : AbstractValidator<AddMediaVideoRequest>
    {
        public AddMediaVideoValidator()
        {
            RuleFor(x => x.VideoTitle).NotEmpty().WithMessage("Video title is required.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");
        }
    }

    public class EditMediaVideoValidator : AbstractValidator<EditMediaVideoRequest>
    {
        private readonly SttbDbContext _db;

        public EditMediaVideoValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.VideoTitle).NotEmpty().WithMessage("Video title is required.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditMediaVideoRequest request, ValidationContext<EditMediaVideoRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "video", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(EditMediaVideoRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class GetMediaVideoValidator : AbstractValidator<GetMediaVideoRequest>
    {
        private readonly SttbDbContext _db;

        public GetMediaVideoValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetMediaVideoRequest request, ValidationContext<GetMediaVideoRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "video", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(GetMediaVideoRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class DeleteMediaVideoValidator : AbstractValidator<DeleteMediaVideoRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaVideoValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(DeleteMediaVideoRequest request, ValidationContext<DeleteMediaVideoRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "video", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(DeleteMediaVideoRequest.Id), "Data doesn't exist");
            }
        }
    }
}
