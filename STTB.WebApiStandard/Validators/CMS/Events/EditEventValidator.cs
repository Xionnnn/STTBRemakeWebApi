using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;

namespace STTB.WebApiStandard.Validators.CMS.Events
{
    public class EditEventValidator : AbstractValidator<EditEventRequest>
    {
        public EditEventValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Event ID must be greater than 0.");
            RuleFor(x => x.EventTitle).NotEmpty().WithMessage("Event Title is required.");
            RuleFor(x => x.StartsAtDate).NotEmpty().WithMessage("Start Date is required.");
        }
    }
}
