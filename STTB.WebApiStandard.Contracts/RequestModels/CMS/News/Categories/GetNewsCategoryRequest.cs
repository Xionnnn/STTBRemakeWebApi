using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.News.Categories;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.News.Categories
{
    public class GetNewsCategoryRequest : IRequest<GetNewsCategoryResponse>
    {
        public long Id { get; set; }
    }
}
