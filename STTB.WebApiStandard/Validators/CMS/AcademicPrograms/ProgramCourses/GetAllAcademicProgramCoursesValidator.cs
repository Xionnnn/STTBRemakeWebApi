using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms.ProgramCourses;

namespace STTB.WebApiStandard.Validators.CMS.AcademicPrograms.ProgramCourses
{
    public class GetAllAcademicProgramCoursesValidator : AbstractValidator<GetAllAcademicProgramCoursesRequest>
    {
        private static readonly string[] AllowedOrderByFields = { "Id", "CourseName", "Credits", "Description", "CreatedAt" };
        private static readonly string[] AllowedOrderStates = { "asc", "desc" };

        public GetAllAcademicProgramCoursesValidator()
        {
            RuleFor(x => x.CourseName)
                .Must(v => v == string.Empty || !string.IsNullOrWhiteSpace(v))
                .WithMessage("CourseName cannot be empty.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

            RuleFor(x => x.OrderBy)
                .Must(v => string.IsNullOrEmpty(v) || AllowedOrderByFields.Contains(v))
                .WithMessage($"OrderBy must be one of: {string.Join(", ", AllowedOrderByFields)}.");

            RuleFor(x => x.OrderState)
                .Must(v => string.IsNullOrEmpty(v) || AllowedOrderStates.Contains(v.ToLower()))
                .WithMessage("OrderState must be 'asc' or 'desc'.");
        }
    }
}
