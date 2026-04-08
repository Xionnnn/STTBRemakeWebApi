using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.Media.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.Media.Categories
{
    public class AddMediaCategoryRequest : IRequest<AddMediaCategoryResponse>
    {
        public string CategoryName { get; set; } = string.Empty;
    }
}
