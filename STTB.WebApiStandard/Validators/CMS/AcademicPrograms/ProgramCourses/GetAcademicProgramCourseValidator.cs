using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.AcademicPrograms.ProgramCourses
{
    public class GetAcademicProgramCourseValidator : AbstractValidator<GetAcademicProgramCoursesRequest>
    {
        private readonly SttbDbContext _db;

        public GetAcademicProgramCourseValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(GetAcademicProgramCoursesRequest request, ValidationContext<GetAcademicProgramCoursesRequest> context, CancellationToken ct)
        {
            var existing = await _db.AcademicProgramCourses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (existing == null)
            {
                context.AddFailure(nameof(GetAcademicProgramCoursesRequest.Id), "Data doesn't exist");
            }
        }
    }
}
