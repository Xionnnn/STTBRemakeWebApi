using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.Web.News
{
    public class GetNewsValidator : AbstractValidator<GetNewsRequest>
    {
        public GetNewsValidator()
        {
            RuleFor(x => x.NewsSlug)
                .NotEmpty()
                .WithMessage("NewsSlug is required.");
        }
    }
}
