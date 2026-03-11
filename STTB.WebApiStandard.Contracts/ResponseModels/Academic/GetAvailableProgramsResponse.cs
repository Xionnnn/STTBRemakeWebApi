using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Academic
{
    public class GetAvailableProgramsResponse
    {
        public IReadOnlyList<ItemDTO> Items { get; set; } = Array.Empty<ItemDTO>();
    }

    public class ItemDTO
    {
        public int? ProgramId { get; set; } = null;
        public string ProgramName { get; set; } = string.Empty;
        public int? Duration { get; set; } = null;
        public string Description { get; set; } = string.Empty;
        public IReadOnlyList<string> Highlight { get; set; } = Array.Empty<string>();
    }

}
