using System;
using System.Collections.Generic;

namespace STTB.WebApiStandard.Entities;

public partial class NewsCategory
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<NewsPostCategory> NewsPostCategories { get; set; } = new List<NewsPostCategory>();
}
