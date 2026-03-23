using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;

namespace STTB.WebApiStandard.Validators.CMS.Lecturers
{
    public class AddLecturerValidator : AbstractValidator<AddLecturerRequest>
    {
        public AddLecturerValidator()
        {
            RuleFor(x => x.LecturerName).NotEmpty().WithMessage("Lecturer Name is required.");
            // Added simple fallback to ensure JoinedAt is provided
            RuleFor(x => x.JoinedAt).NotEmpty().WithMessage("Joined At date is required.");
        }
    }
}
