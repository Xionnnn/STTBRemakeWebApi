using MediatR;
using STTB.WebApiStandard.Contracts.ResponseModels.CMS.AcademicPrograms;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.CMS.AcademicPrograms
{
    public class GetAcademicProgramRequest : IRequest<GetAcademicProgramResponse>
    {
        public long Id { get; set; }
    }
}
