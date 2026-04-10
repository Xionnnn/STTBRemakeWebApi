using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    internal static class MediaValidationConstants
    {
        public static readonly string[] AllowedImageMimeTypes = { "image/jpeg", "image/png", "image/jpg", "image/gif", "image/webp" };
    }

    public class AddMediaJournalValidator : AbstractValidator<AddMediaJournalRequest>
    {
        public AddMediaJournalValidator()
        {
            RuleFor(x => x.JournalTitle).NotEmpty().WithMessage("Journal title is required.");

            RuleFor(x => x.JournalFile)
                .Must(f => f == null || f.ContentType == "application/pdf")
                .WithMessage("Journal file must be in PDF format.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");
        }
    }

    public class EditMediaJournalValidator : AbstractValidator<EditMediaJournalRequest>
    {
        private readonly SttbDbContext _db;

        public EditMediaJournalValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.JournalTitle).NotEmpty().WithMessage("Journal title is required.");

            RuleFor(x => x.JournalFile)
                .Must(f => f == null || f.ContentType == "application/pdf")
                .WithMessage("Journal file must be in PDF format.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditMediaJournalRequest request, ValidationContext<EditMediaJournalRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "journal", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(EditMediaJournalRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class GetMediaJournalValidator : AbstractValidator<GetMediaJournalRequest>
    {
        private readonly SttbDbContext _db;

        public GetMediaJournalValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetMediaJournalRequest request, ValidationContext<GetMediaJournalRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "journal", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(GetMediaJournalRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class DeleteMediaJournalValidator : AbstractValidator<DeleteMediaJournalRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaJournalValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(DeleteMediaJournalRequest request, ValidationContext<DeleteMediaJournalRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "journal", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(DeleteMediaJournalRequest.Id), "Data doesn't exist");
            }
        }
    }
}
