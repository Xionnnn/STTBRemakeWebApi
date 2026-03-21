using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;
using System.Linq;

namespace STTB.WebApiStandard.Validators.Media
{
    public class GetAvailableMediaValidator : AbstractValidator<GetAvailableMediaRequest>
    {
        private readonly string[] _allowedOrderBy = { "VideoTitle", "AuthorName" };
        private readonly string[] _allowedOrderState = { "asc", "desc" };

        public GetAvailableMediaValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must not exceed 100");

            RuleFor(x => x.FetchLimit)
                .GreaterThan(0)
                .When(x => x.FetchLimit.HasValue)
                .WithMessage("FetchLimit must be greater than 0");

            RuleFor(x => x.OrderBy)
                .Must(x => string.IsNullOrEmpty(x) || _allowedOrderBy.Contains(x))
                .WithMessage($"OrderBy must be one of: {string.Join(", ", _allowedOrderBy)}");

            RuleFor(x => x.OrderState)
                .Must(x => string.IsNullOrEmpty(x) || _allowedOrderState.Contains(x.ToLower()))
                .WithMessage($"OrderState must be one of: {string.Join(", ", _allowedOrderState)}");
        }
    }
}
