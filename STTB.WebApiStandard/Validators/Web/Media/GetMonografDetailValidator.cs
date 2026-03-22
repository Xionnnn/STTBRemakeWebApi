using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.Web.Media
{
    public class GetMonografDetailValidator: AbstractValidator<GetMonografDetailRequest>
    {
        public GetMonografDetailValidator()
        {
            RuleFor(x => x.MonografSlug)
                .NotEmpty()
                .WithMessage("MonografSlug is required");
        }
    }
}
