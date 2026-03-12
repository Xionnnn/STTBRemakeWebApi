using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.Events
{
    public class GetEventValidator : AbstractValidator<GetEventRequest>
    {
        public GetEventValidator()
        {
            RuleFor(x => x.EventSlug)
                .NotEmpty()
                .WithMessage("EventSlug is required.");
        }
    }
}
