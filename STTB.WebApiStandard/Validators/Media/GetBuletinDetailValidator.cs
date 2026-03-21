using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.Web.Media;

namespace STTB.WebApiStandard.Validators.Media
{
    public class GetBuletinDetailValidator : AbstractValidator<GetBuletinDetailRequest>
    {
        public GetBuletinDetailValidator()
        {
            RuleFor(x => x.BuletinSlug)
                .NotEmpty()
                .WithMessage("BuletinSlug is required");
        }
    }
}
