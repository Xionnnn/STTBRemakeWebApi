using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;

namespace STTB.WebApiStandard.Validators.CMS.AcademicPrograms
{
    public class EditAcademicProgramValidator : AbstractValidator<EditAcademicProgramRequest>
    {
        public EditAcademicProgramValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Academic Program ID must be greater than 0.");
            RuleFor(x => x.ProgramName).NotEmpty().WithMessage("ProgramName is required.");
            RuleFor(x => x.Degree).NotEmpty().WithMessage("Degree is required.");
        }
    }
}
