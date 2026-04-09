using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionCosts.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AdmissionCosts.Categories
{
    public class GetAllCostCategoryRequest : IRequest<GetAllCostCategoryResponse>
    {
        public string CategoryName { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public string OrderState { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool FetchAll { get; set; } = false;
    }
}
