using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AcademicPrograms.ProgramCourses
{
    public class AddAcademicProgramCourseValidator : AbstractValidator<AddAcademicProgramCoursesRequest>
    {
        private readonly SttbDbContext _db;

        public AddAcademicProgramCourseValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.CourseName)
                .NotEmpty().WithMessage("Course Name is required.");

            RuleFor(x => x.Credits)
                .GreaterThan(0).WithMessage("Credits must be greater than 0.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddAcademicProgramCoursesRequest request, ValidationContext<AddAcademicProgramCoursesRequest> context, CancellationToken ct)
        {
            var existing = await _db.AcademicProgramCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToUpper() == request.CourseName.ToUpper(), ct);

            if (existing != null)
            {
                context.AddFailure(nameof(AddAcademicProgramCoursesRequest.CourseName), "Data Already Exist");
            }
        }
    }
}
