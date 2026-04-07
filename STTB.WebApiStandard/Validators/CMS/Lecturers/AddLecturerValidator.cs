using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Lecturers;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Lecturers
{
    public class AddLecturerValidator : AbstractValidator<AddLecturerRequest>
    {
        private readonly SttbDbContext _db;
        public AddLecturerValidator(SttbDbContext db)
        {
            _db = db;
            RuleFor(x => x.LecturerName).NotEmpty().WithMessage("Lecturer Name is required.");
            RuleFor(x => x.JoinedAt).NotEmpty().WithMessage("Joined At date is required.");
            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddLecturerRequest request, ValidationContext<AddLecturerRequest> context, CancellationToken ct)
        {
            var existingLecturer = await _db.Lecturers
                .FirstOrDefaultAsync(l => l.LecturerName.ToUpper() == request.LecturerName.ToUpper(), ct);

            if (existingLecturer != null)
            {
                context.AddFailure(nameof(AddLecturerRequest), "Data Already Exist");
            }
        }
    }
}
