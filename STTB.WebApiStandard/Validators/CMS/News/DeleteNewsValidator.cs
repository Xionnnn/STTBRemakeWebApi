using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News;

namespace STTB.WebApiStandard.Validators.CMS.News
{
    public class DeleteNewsValidator : AbstractValidator<DeleteNewsRequest>
    {
        public DeleteNewsValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("News ID must be greater than 0.");
        }
    }
}
