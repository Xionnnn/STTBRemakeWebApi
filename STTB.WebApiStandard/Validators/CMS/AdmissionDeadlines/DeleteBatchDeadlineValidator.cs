using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionDeadlines
{
    public class DeleteBatchDeadlineValidator : AbstractValidator<DeleteBatchDeadlineRequest>
    {
        public DeleteBatchDeadlineValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Batch deadline ID must be greater than 0.");
        }
    }
}
