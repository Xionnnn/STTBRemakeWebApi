using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;

namespace STTB.WebApiStandard.Validators.CMS.News
{
    public class GetAllNewsValidator : AbstractValidator<GetAllNewsRequest>
    {
        private static readonly string[] AllowedOrderByFields =
        {
            "Id", "Title", "PublicationDate", "CreatedAt"
        };

        private static readonly string[] AllowedOrderStates = { "asc", "desc" };

        public GetAllNewsValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.OrderBy)
                .Must(v => string.IsNullOrEmpty(v) || AllowedOrderByFields.Contains(v))
                .WithMessage($"OrderBy must be one of: {string.Join(", ", AllowedOrderByFields)}.");

            RuleFor(x => x.OrderState)
                .Must(v => string.IsNullOrEmpty(v) || AllowedOrderStates.Contains(v.ToLower()))
                .WithMessage("OrderState must be 'asc' or 'desc'.");
        }
    }
}
