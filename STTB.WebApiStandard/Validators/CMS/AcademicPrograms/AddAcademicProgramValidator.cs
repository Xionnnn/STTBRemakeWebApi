using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;

namespace STTB.WebApiStandard.Validators.CMS.AcademicPrograms
{
    public class AddAcademicProgramValidator : AbstractValidator<AddAcademicProgramRequest>
    {
        public AddAcademicProgramValidator()
        {
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage("Program Name is required.");
            RuleFor(x => x.Slug).NotEmpty().WithMessage("Slug is required.");
            RuleFor(x => x.Degree).NotEmpty().WithMessage("Degree is required.");
            RuleFor(x => x.TotalCredits).GreaterThanOrEqualTo(0).When(x => x.TotalCredits.HasValue).WithMessage("Total Credits must be non-negative.");
            RuleFor(x => x.Duration).GreaterThanOrEqualTo(0).When(x => x.Duration.HasValue).WithMessage("Duration must be non-negative.");
        }
    }
}
