using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;

namespace STTB.WebApiStandard.Validators.CMS.Lecturers
{
    public class EditLecturerValidator : AbstractValidator<EditLecturerRequest>
    {
        public EditLecturerValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lecturer ID must be greater than 0.");
            
            RuleFor(x => x.LecturerName)
                .NotEmpty().WithMessage("LecturerName is required.")
                .MaximumLength(255).WithMessage("LecturerName cannot exceed 255 characters.");

            RuleFor(x => x.OrganizationalRole)
                .NotEmpty().WithMessage("OrganizationalRole is required.");

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roles list cannot be null.");

            RuleFor(x => x.Degrees)
                .NotNull().WithMessage("Degrees list cannot be null.");

            // Optional Image validation (size, type)
            When(x => x.LecturerImage != null, () =>
            {
                RuleFor(x => x.LecturerImage!)
                    .Must(file => file.Length <= 5 * 1024 * 1024)
                    .WithMessage("Image size must not exceed 5MB.")
                    .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/webp")
                    .WithMessage("Only JPEG, PNG, and WEBP images are allowed.");
            });
        }
    }
}
