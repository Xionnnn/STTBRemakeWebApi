using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories
{
    public class EditCostCategoryRequest : IRequest<EditCostCategoryResponse>
    {
        public long Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
