using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Articles;

namespace STTB.WebApiStandard.Validators.CMS.Media.Articles
{
    public class AddMediaArticleValidator : AbstractValidator<AddMediaArticleRequest>
    {
        public AddMediaArticleValidator()
        {
            RuleFor(x => x.ArticleTitle).NotEmpty().WithMessage("Article title is required.");
        }
    }

    public class EditMediaArticleValidator : AbstractValidator<EditMediaArticleRequest>
    {
        public EditMediaArticleValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.ArticleTitle).NotEmpty().WithMessage("Article title is required.");
        }
    }
}
