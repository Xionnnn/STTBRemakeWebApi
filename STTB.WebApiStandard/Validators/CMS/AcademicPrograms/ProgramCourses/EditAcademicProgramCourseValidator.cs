using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AcademicPrograms.ProgramCourses
{
    public class EditAcademicProgramCourseValidator : AbstractValidator<EditAcademicProgramCoursesRequest>
    {
        private readonly SttbDbContext _db;

        public EditAcademicProgramCourseValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x.CourseName)
                .NotEmpty().WithMessage("Course Name is required.");

            RuleFor(x => x.Credits)
                .GreaterThan(0).WithMessage("Credits must be greater than 0.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(EditAcademicProgramCoursesRequest request, ValidationContext<EditAcademicProgramCoursesRequest> context, CancellationToken ct)
        {
            var existing = await _db.AcademicProgramCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (existing == null)
            {
                context.AddFailure(nameof(EditAcademicProgramCoursesRequest.Id), "Data doesn't exist");
                return;
            }

            // Exclude current record from uniqueness check
            var duplicate = await _db.AcademicProgramCourses
                .FirstOrDefaultAsync(c => c.Id != request.Id &&
                    c.Name.ToUpper() == request.CourseName.ToUpper(), ct);

            if (duplicate != null)
            {
                context.AddFailure(nameof(EditAcademicProgramCoursesRequest.CourseName), "The inputted name has already existed");
            }
        }
    }
}
