using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.Web.Events
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
