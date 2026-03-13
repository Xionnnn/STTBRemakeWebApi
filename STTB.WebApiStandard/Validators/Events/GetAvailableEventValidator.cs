using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.Events
{
    public class GetAvailableEventValidator : AbstractValidator<GetAvailableEventRequest>
    {
        private static readonly string[] AllowedOrderBy = ["EventTitle", "CategoryName"];
        private static readonly string[] AllowedOrderState = ["asc", "desc"];

        public GetAvailableEventValidator()
        {
            RuleFor(x => x.OrderBy)
                .Must(value => AllowedOrderBy.Contains(value))
                .When(x => !string.IsNullOrEmpty(x.OrderBy))
                .WithMessage($"OrderBy must be one of: {string.Join(", ", AllowedOrderBy)}.");

            RuleFor(x => x.OrderState)
                .Must(value => AllowedOrderState.Contains(value))
                .When(x => !string.IsNullOrEmpty(x.OrderState))
                .WithMessage($"OrderState must be one of: {string.Join(", ", AllowedOrderState)}.");

            RuleFor(x => x.OrderState)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.OrderBy))
                .WithMessage("OrderState is required when OrderBy is provided.");

            RuleFor(x => x.OrderBy)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.OrderState))
                .WithMessage("OrderBy is required when OrderState is provided.");

            RuleFor(x => x.EventDate)
                .NotNull()
                .NotEqual(default(DateTime))
                .When(x => x.EventDate.HasValue)
                .WithMessage("EventDate cannot be empty when provided.");

            RuleFor(x => x.EventTitle)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .When(x => x.EventTitle != string.Empty)
                .WithMessage("EventTitle cannot be null or contain only whitespace when provided.");

            RuleFor(x => x.CategoryName)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .When(x => x.CategoryName != string.Empty)
                .WithMessage("CategoryName cannot be null or contain only whitespace when provided.");

            RuleFor(x => x.OrganizerName)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .When(x => x.OrganizerName != string.Empty)
                .WithMessage("OrganizerName cannot be null or contain only whitespace when provided.");

            RuleFor(x => x.FetchLimit)
                .GreaterThan(0)
                .When(x => x.FetchLimit.HasValue)
                .WithMessage("FetchLimit must be greater than 0 when provided.");
        }
    }
}
