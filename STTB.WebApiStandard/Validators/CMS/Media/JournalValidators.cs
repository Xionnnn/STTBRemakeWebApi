using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Journals;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    public class AddMediaJournalValidator : AbstractValidator<AddMediaJournalRequest>
    {
        public AddMediaJournalValidator()
        {
            RuleFor(x => x.JournalTitle).NotEmpty().WithMessage("Journal title is required.");
        }
    }

    public class EditMediaJournalValidator : AbstractValidator<EditMediaJournalRequest>
    {
        public EditMediaJournalValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.JournalTitle).NotEmpty().WithMessage("Journal title is required.");
        }
    }
}
