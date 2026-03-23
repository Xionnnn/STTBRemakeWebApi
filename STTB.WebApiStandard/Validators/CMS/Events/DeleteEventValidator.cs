using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Events;

namespace STTB.WebApiStandard.Validators.CMS.Events
{
    public class DeleteEventValidator : AbstractValidator<DeleteEventRequest>
    {
        public DeleteEventValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Event ID must be greater than 0.");
        }
    }
}
