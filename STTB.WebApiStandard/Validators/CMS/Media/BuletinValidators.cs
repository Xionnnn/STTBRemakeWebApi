using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Buletins;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    public class AddMediaBuletinValidator : AbstractValidator<AddMediaBuletinRequest>
    {
        public AddMediaBuletinValidator()
        {
            RuleFor(x => x.BuletinTitle).NotEmpty().WithMessage("Buletin title is required.");

            RuleFor(x => x.BuletinFile)
                .Must(f => f == null || f.ContentType == "application/pdf")
                .WithMessage("Buletin file must be in PDF format.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");
        }
    }

    public class EditMediaBuletinValidator : AbstractValidator<EditMediaBuletinRequest>
    {
        private readonly SttbDbContext _db;

        public EditMediaBuletinValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.BuletinTitle).NotEmpty().WithMessage("Buletin title is required.");

            RuleFor(x => x.BuletinFile)
                .Must(f => f == null || f.ContentType == "application/pdf")
                .WithMessage("Buletin file must be in PDF format.");

            RuleFor(x => x.Thumbnail)
                .Must(f => f == null || MediaValidationConstants.AllowedImageMimeTypes.Contains(f.ContentType.ToLower()))
                .WithMessage("Thumbnail must be an image file (jpg, jpeg, png, gif, webp).");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditMediaBuletinRequest request, ValidationContext<EditMediaBuletinRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "buletin", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(EditMediaBuletinRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class GetMediaBuletinValidator : AbstractValidator<GetMediaBuletinRequest>
    {
        private readonly SttbDbContext _db;

        public GetMediaBuletinValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetMediaBuletinRequest request, ValidationContext<GetMediaBuletinRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "buletin", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(GetMediaBuletinRequest.Id), "Data doesn't exist");
            }
        }
    }

    public class DeleteMediaBuletinValidator : AbstractValidator<DeleteMediaBuletinRequest>
    {
        private readonly SttbDbContext _db;

        public DeleteMediaBuletinValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(DeleteMediaBuletinRequest request, ValidationContext<DeleteMediaBuletinRequest> context, CancellationToken ct)
        {
            var existing = await _db.MediaItems
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == request.Id && m.MediaFormat == "buletin", ct);

            if (existing == null)
            {
                context.AddFailure(nameof(DeleteMediaBuletinRequest.Id), "Data doesn't exist");
            }
        }
    }
}
