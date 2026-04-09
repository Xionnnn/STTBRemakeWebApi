using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Academics;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.Web.Academics
{
    public class GetAvailableProgramValidator : AbstractValidator<GetAllAcademicProgramRequest>
    {
        public GetAvailableProgramValidator()
        {
            RuleFor(x => x.FetchLimit)
                .GreaterThan(0)
                .When(x => x.FetchLimit.HasValue)
                .WithMessage("FetchLimit must be greater than 0 when provided.");
        }
    }
}
