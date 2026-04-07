using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Events
{
    public class AddEventValidator : AbstractValidator<AddEventRequest>
    {
        private readonly SttbDbContext _db;
        public AddEventValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.EventTitle).NotEmpty().WithMessage("Event Title is required.");
            RuleFor(x => x.StartsAtDate).NotEmpty().WithMessage("Start Date is required.");
            RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required.");
            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }
        private async Task ValidateBusinessAsync(AddEventRequest request, ValidationContext<AddEventRequest> context, CancellationToken ct)
        {
            var existingEvents= await _db.Events
                .FirstOrDefaultAsync(e => e.Slug == request.Slug || e.Title.ToUpper() == request.EventTitle.ToUpper());

            if (existingEvents != null)
            {
                context.AddFailure(nameof(AddEventRequest), "Data Already Exist");
            }
        }
    }
}
