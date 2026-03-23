using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;

namespace STTB.WebApiStandard.Validators.CMS.News
{
    public class EditNewsValidator : AbstractValidator<EditNewsRequest>
    {
        public EditNewsValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("News ID must be greater than 0.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("News Title is required.");
        }
    }
}
