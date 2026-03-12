using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Academic
{
    public class GetAvailableProgramResponse
    {
        public IReadOnlyList<ItemDTO> Items { get; set; } = Array.Empty<ItemDTO>();
    }

    public class ItemDTO
    {
        public int? ProgramId { get; set; } = null;
        public string ProgramName { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public int? Duration { get; set; } = null;
        public int? TotalCredit { get; set; } = null;
    }

}
