using STTB.WebApiStandard.Contracts.DTOs.Web.Academics;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Academic
{
    public class GetAllAcademicProgramResponse
    {
        public List<GetAllAcademicDTO> Items { get; set; } = new List<GetAllAcademicDTO>();
    }
}
