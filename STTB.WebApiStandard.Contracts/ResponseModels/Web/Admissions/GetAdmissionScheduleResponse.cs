using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Admissions
{
    public class GetAdmissionScheduleResponse
    {
        public DateTime FirstBatchDeadline { get; set; }
        public DateTime SecondBatchDeadline { get; set; }
        public DateTime? ThirdBatchDeadline { get; set; }
    }
}
