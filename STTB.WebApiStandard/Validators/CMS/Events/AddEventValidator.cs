using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;

namespace STTB.WebApiStandard.Validators.CMS.Events
{
    public class AddEventValidator : AbstractValidator<AddEventRequest>
    {
        public AddEventValidator()
        {
            RuleFor(x => x.EventTitle).NotEmpty().WithMessage("Event Title is required.");
            RuleFor(x => x.StartsAtDate).NotEmpty().WithMessage("Start Date is required.");
            RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required.");
        }
    }
}
