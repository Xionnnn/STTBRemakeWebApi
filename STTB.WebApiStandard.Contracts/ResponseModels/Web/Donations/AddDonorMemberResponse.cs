using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace STTB.WebApiStandard.Contracts.ResponseModels.Donations
{
    public class AddDonorMemberResponse
    {
        public string DonorId { get; set; } = string.Empty;
        public string DonorName { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
    }
}
