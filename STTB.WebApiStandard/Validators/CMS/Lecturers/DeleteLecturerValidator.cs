using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;

namespace STTB.WebApiStandard.Validators.CMS.Lecturers
{
    public class DeleteLecturerValidator : AbstractValidator<DeleteLecturerRequest>
    {
        public DeleteLecturerValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Lecturer ID must be greater than 0.");
        }
    }
}
