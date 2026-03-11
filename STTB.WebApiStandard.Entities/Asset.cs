using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class Asset
{
    public long Id { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public string MimeType { get; set; } = null!;

    public string? Title { get; set; }

    public string? AltText { get; set; }

    public long? SizeBytes { get; set; }

    public int? Width { get; set; }

    public int? Height { get; set; }

    public string? ModelType { get; set; }

    public long? ModelId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
