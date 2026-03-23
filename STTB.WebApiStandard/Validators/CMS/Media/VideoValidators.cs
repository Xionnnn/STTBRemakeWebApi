using FluentValidation;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Videos;

namespace STTB.WebApiStandard.Validators.CMS.Media
{
    public class AddMediaVideoValidator : AbstractValidator<AddMediaVideoRequest>
    {
        public AddMediaVideoValidator()
        {
            RuleFor(x => x.VideoTitle).NotEmpty().WithMessage("Video title is required.");
        }
    }

    public class EditMediaVideoValidator : AbstractValidator<EditMediaVideoRequest>
    {
        public EditMediaVideoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Valid Id is required.");
            RuleFor(x => x.VideoTitle).NotEmpty().WithMessage("Video title is required.");
        }
    }
}
