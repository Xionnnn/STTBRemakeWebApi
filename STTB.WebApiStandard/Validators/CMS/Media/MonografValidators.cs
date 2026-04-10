using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Monograf;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    public class AddMediaMonografValidator : AbstractValidator<AddMediaMonografRequest>
    {
        public AddMediaMonografValidator()
        {
            RuleFor(x => x.MonografTitle).NotEmpty().WithMessage("Monograf title is required.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");
        }
    }

    public class EditMediaMonografValidator : AbstractValidator<EditMediaMonografRequest>
    {
        private readonly SttbDbContext _db;

        public EditMediaMonografValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.MonografTitle).NotEmpty().WithMessage("Monograf title is required.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditMediaMonografRequest request, ValidationContext<EditMediaMonografRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "monograf", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(EditMediaMonografRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class GetMediaMonografValidator : AbstractValidator<GetMediaMonografRequest>
    {
        private readonly SttbDbContext _db;

        public GetMediaMonografValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetMediaMonografRequest request, ValidationContext<GetMediaMonografRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "monograf", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(GetMediaMonografRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class DeleteMediaMonografValidator : AbstractValidator<DeleteMediaMonografRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaMonografValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(DeleteMediaMonografRequest request, ValidationContext<DeleteMediaMonografRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "monograf", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(DeleteMediaMonografRequest.Id), "Data doesn't exist");
            }
        }
    }
}
