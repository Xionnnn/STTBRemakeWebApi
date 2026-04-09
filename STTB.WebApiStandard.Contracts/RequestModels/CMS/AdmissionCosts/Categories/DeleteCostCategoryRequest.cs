using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories
{
    public class DeleteCostCategoryRequest : IRequest<DeleteCostCategoryResponse>
    {
        public long Id { get; set; }
    }
}
