using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class NewsPost
{
    public long Id { get; set; }

    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime PublishedAt { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<NewsPostCategory> NewsPostCategories { get; set; } = new List<NewsPostCategory>();
}
