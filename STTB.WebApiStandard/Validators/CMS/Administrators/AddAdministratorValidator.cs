using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Administrators;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Administrators
{
    public class AddAdministratorValidator : AbstractValidator<AddAdministratorRequest>
    {
        private readonly SttbDbContext _db;

        public AddAdministratorValidator(SttbDbContext db)
        {
            _db = db;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Administrator Name is required.");
            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }

        private async Task ValidateBusinessAsync(AddAdministratorRequest request, ValidationContext<AddAdministratorRequest> context, CancellationToken ct)
        {
            var existingAdmin = await _db.FoundationAdministrators
                .FirstOrDefaultAsync(l => l.AdminName.ToUpper() == request.Name.ToUpper(), ct);

            if (existingAdmin != null)
            {
                context.AddFailure(nameof(AddAdministratorRequest), "Data Already Exist");
            }
        }
    }
}
