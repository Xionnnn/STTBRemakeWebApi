using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;

namespace STTB.WebApiStandard.Validators.Media
{
    public class GetVideoDetailValidator : AbstractValidator<GetVideoDetailRequest>
    {
        public GetVideoDetailValidator()
        {
            RuleFor(x => x.VideoSlug)
                .NotEmpty()
                .WithMessage("VideoSlug is required");
        }
    }
}
