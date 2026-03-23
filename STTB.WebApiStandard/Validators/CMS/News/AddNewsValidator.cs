using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;

namespace STTB.WebApiStandard.Validators.CMS.News
{
    public class AddNewsValidator : AbstractValidator<AddNewsRequest>
    {
        public AddNewsValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("News Title is required.");
            RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required.");
        }
    }
}
