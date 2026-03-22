using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Academics
{
    public class GetAcademicRequirementsResponse
    {
        public IReadOnlyList<ProgramRequirementDto> Items { get; set; } = Array.Empty<ProgramRequirementDto>();
    }

    public class ProgramRequirementDto
    {
        public string ProgramName { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public IReadOnlyList<string> Requirements { get; set; } = Array.Empty<string>();
    }
}
