using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Media;

namespace STTB.WebApiStandard.Validators.Media
{
    public class GetArticleDetailValidator : AbstractValidator<GetArticleDetailRequest>
    {
        public GetArticleDetailValidator()
        {
            RuleFor(x => x.ArticleSlug)
                .NotEmpty()
                .WithMessage("ArticleSlug is required");
        }
    }
}
