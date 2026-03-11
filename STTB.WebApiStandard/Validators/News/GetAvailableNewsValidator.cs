using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.News
{
    public class GetAvailableNewsValidator : AbstractValidator<GetAvailableNewsRequest>
    {
        private static readonly string[] AllowedOrderBy = ["NewsTitle", "CategoryName"];
        private static readonly string[] AllowedOrderState = ["asc", "desc"];

        public GetAvailableNewsValidator()
        {
            // OrderBy: when provided, must be an allowed value
            RuleFor(x => x.OrderBy)
                .Must(value => AllowedOrderBy.Contains(value))
                .When(x => !string.IsNullOrEmpty(x.OrderBy))
                .WithMessage($"OrderBy must be one of: {string.Join(", ", AllowedOrderBy)}.");

            // OrderState: when provided, must be an allowed value
            RuleFor(x => x.OrderState)
                .Must(value => AllowedOrderState.Contains(value))
                .When(x => !string.IsNullOrEmpty(x.OrderState))
                .WithMessage($"OrderState must be one of: {string.Join(", ", AllowedOrderState)}.");

            // OrderBy and OrderState must co-exist
            RuleFor(x => x.OrderState)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.OrderBy))
                .WithMessage("OrderState is required when OrderBy is provided.");

            RuleFor(x => x.OrderBy)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.OrderState))
                .WithMessage("OrderBy is required when OrderState is provided.");

            // EventDate: if provided, it cannot be default (null is the absent state,
            // but since it's DateTime?, HasValue means it was supplied)
            RuleFor(x => x.EventDate)
                .NotNull()
                .NotEqual(default(DateTime))
                .When(x => x.EventDate.HasValue)
                .WithMessage("EventDate cannot be empty when provided.");

            // NewsTitle: if provided, cannot be whitespace-only
            RuleFor(x => x.NewsTitle)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .When(x => x.NewsTitle != string.Empty)
                .WithMessage("NewsTitle cannot be null or contain only whitespace when provided.");

            // CategoryName: if provided, cannot be whitespace-only
            RuleFor(x => x.CategoryName)
                .Must(value => !string.IsNullOrWhiteSpace(value))
                .When(x => x.CategoryName != string.Empty)
                .WithMessage("CategoryName cannot be null or contain only whitespace when provided.");

            // FetchLimit: if provided, must be > 0
            RuleFor(x => x.FetchLimit)
                .GreaterThan(0)
                .When(x => x.FetchLimit.HasValue)
                .WithMessage("FetchLimit must be greater than 0 when provided.");
        }
    }
}
