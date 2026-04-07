using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Permissions;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;
using STTB.WebApiStandard.Entities;

namespace STTB.WebApiStandard.Validators.CMS.Users.Permissions
{
    public class AddPermissionValidator : AbstractValidator<AddPermissionRequest>
    {
        private readonly SttbDbContext _db;
        public AddPermissionValidator(SttbDbContext db)
        {
            _db = db;

            RuleFor(x => x.PermissionName)
                .NotEmpty().WithMessage("PermissionName is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }
        private async Task ValidateBusinessAsync(AddPermissionRequest request, ValidationContext<AddPermissionRequest> context, CancellationToken ct)
        {
            var existingPermissions = await _db.Permissions
                .FirstOrDefaultAsync(p => p.Name.ToUpper() == request.PermissionName.ToUpper(), ct);

            if(existingPermissions != null)
            {
                context.AddFailure(nameof(AddPermissionRequest.PermissionName), "Data Already Exist");
            }
        }
    }
}
