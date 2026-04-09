using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.RequestModels.Web.Academics
{
    public class AcademicProgramRequirementsDTO
    {
        public string ProgramName { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public List<string> Requirements { get; set; } = new List<string>();
    }
}
