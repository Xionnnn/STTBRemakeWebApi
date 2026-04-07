using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Users.Roles;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.CMS.Users.Roles
{
    public class AddUserRoleValidator : AbstractValidator<AddUserRoleRequest>
    {
        private readonly SttbDbContext _db;

        public AddUserRoleValidator(SttbDbContext db)
        {
            _db = db;


            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("RoleName is required.");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }
        private async Task ValidateBusinessAsync (AddUserRoleRequest request, ValidationContext<AddUserRoleRequest> context, CancellationToken ct)
        {
            var existingRole = await _db.Roles
                .FirstOrDefaultAsync(r => r.Name.ToUpper() == request.RoleName.ToUpper(), ct);

            if(existingRole != null)
            {
                context.AddFailure(nameof(AddUserRoleRequest.RoleName), "Data already exist");
            }
        }
    }
}
