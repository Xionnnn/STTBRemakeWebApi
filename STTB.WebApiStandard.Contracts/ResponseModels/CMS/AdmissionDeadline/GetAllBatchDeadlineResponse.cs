using System;
using System.Collections.Generic;
using System.Text;
using STTB.WebApiStandard.Contracts.DTOs.CMS.AdmissionDeadline;

namespace STTB.WebApiStandard.Contracts.ResponseModels.CMS.AdmissionDeadline
{
    public class GetAllBatchDeadlineResponse
    {
        public List<CMSGetAllBatchDeadlineDTO> Items { get; set; } = new List<CMSGetAllBatchDeadlineDTO>();
    }
}
