using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Academics;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.Web.Academics
{
    public class GetProgramValidator : AbstractValidator<GetProgramRequest>
    {
        public GetProgramValidator()
        {
            RuleFor(x => x.ProgramSlug)
                .NotEmpty()
                .WithMessage("ProgramSlug is required.");
        }
    }
}
