using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionDeadline;

namespace STTB.WebApiStandard.Validators.CMS.AdmissionDeadlines
{
    public class EditBatchDeadlineValidator : AbstractValidator<EditBatchDeadlineRequest>
    {
        public EditBatchDeadlineValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Batch deadline ID must be greater than 0.");

            RuleFor(x => x.AcademicYear)
                .NotEmpty().WithMessage("AcademicYear is required.");

            RuleFor(x => x.BatchOrder)
                .GreaterThan(0).WithMessage("BatchOrder must be greater than 0.");
        }
    }
}
