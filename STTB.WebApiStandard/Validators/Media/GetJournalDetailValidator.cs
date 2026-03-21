using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;

namespace STTB.WebApiStandard.Validators.Media
{
    public class GetJournalDetailValidator : AbstractValidator<GetJournalDetailRequest>
    {
        public GetJournalDetailValidator()
        {
            RuleFor(x => x.JournalSlug)
                .NotEmpty()
                .WithMessage("JournalSlug is required");
        }
    }
}
