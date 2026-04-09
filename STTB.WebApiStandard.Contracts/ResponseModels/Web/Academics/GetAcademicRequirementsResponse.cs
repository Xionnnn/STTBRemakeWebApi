using STTB.WebApiStandard.Contracts.RequestModels.Web.Academics;
using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Academics
{
    public class GetAcademicRequirementsResponse
    {
        public List<AcademicProgramRequirementsDTO> Items { get; set; } = new List<AcademicProgramRequirementsDTO>();
    }

}
