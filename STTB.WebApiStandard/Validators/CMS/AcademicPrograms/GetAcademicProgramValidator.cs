using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms;

namespace STTB.WebApiStandard.Validators.CMS.AcademicPrograms
{
    public class GetAcademicProgramValidator : AbstractValidator<GetAcademicProgramRequest>
    {
        public GetAcademicProgramValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Academic Program ID must be greater than 0.");
        }
    }
}
