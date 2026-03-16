using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class DonorMember
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Salutation { get; set; }

    public string? Contact { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string DonationType { get; set; } = null!;

    public string DonationArea { get; set; } = null!;

    public bool ProofOfSupport { get; set; }

    public decimal DonationAmount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual DonorScholarshipDetail? DonorScholarshipDetail { get; set; }
}
