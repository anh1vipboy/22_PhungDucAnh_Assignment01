using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Models;

public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
