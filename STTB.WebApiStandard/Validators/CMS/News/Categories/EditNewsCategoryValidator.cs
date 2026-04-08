using FluentValidation;
using Microsoft.EntityFrameworkCore;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Validators.CMS.News.Categories
{
    public class EditNewsCategoryValidator : AbstractValidator<EditNewsCategoryRequest>
    {

        private readonly SttbDbContext _db;
        public EditNewsCategoryValidator(SttbDbContext db)
        {
            _db = db;
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be provided and have to more than 0");

            RuleFor(x => x.CategoryName)
                .Must(v => v == string.Empty || !string.IsNullOrEmpty(v))
                .WithMessage("CategoryName cannot be empty");

            RuleFor(x => x).CustomAsync(ValidateBusinessAsync);
        }
        private async Task ValidateBusinessAsync(EditNewsCategoryRequest request, ValidationContext<EditNewsCategoryRequest> context, CancellationToken ct)
        {
            var newsCategory = await _db.NewsCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id, ct);

            if (newsCategory == null)
            {
                context.AddFailure(nameof(EditNewsCategoryRequest.Id), "Data doesn't exist");
            }

            // Exclude the current ID from the uniqueness check so we don't conflict with our own name
            var existingName = await _db.NewsCategories
                .FirstOrDefaultAsync(nc => nc.Id != request.Id && nc.Name.ToUpper() == request.CategoryName.ToUpper(), ct);

            if (existingName != null)
            {
                context.AddFailure(nameof(EditNewsCategoryRequest.CategoryName), "The inputted name has already existed");
            }
        }
    }
}
    
