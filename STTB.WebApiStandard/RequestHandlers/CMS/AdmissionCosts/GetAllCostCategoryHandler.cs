using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts;
using STTB.WebApiStandard.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.RequestHandlers.CMS.AdmissionCosts
{
    public class GetAllCostCategoryHandler : IRequestHandler<GetAllCostCategoryRequest,GetAllCostCategoryResponse>
    {
        private readonly SttbDbContext _db;
        private readonly ILogger<GetAllCostCategoryHandler> _logger;

        public GetAllCostCategoryHandler(SttbDbContext db, ILogger<GetAllCostCategoryHandler> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<GetAllCostCategoryResponse> Handle(GetAllCostCategoryRequest request, CancellationToken ct)
        {
            var categories = await _db.AcademicProgramCostCategories.AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .Select(c=> new CMSCostCategoryDTO
                {
                    Id = c.Id,
                    CategoryName =  c.CategoryName
                })
                .ToListAsync(ct);

            return new GetAllCostCategoryResponse
            {
                Items = categories
            };
        }

    }
}
