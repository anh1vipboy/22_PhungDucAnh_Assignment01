using System.ComponentModel.DataAnnotations;

namespace _22_PhungDucAnh_Assignment01_FrontEnd.Models;

public class TagViewModel
{
    public int TagId { get; set; }
    public string? TagName { get; set; }
    public string? Note { get; set; }
}

public class CategoryViewModel
{
    public short CategoryId { get; set; }

    [Required(ErrorMessage = "Category Name is required")]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(250)]
    public string CategoryDesciption { get; set; } = string.Empty;

    public short? ParentCategoryId { get; set; }
    public bool? IsActive { get; set; } = true;
}

public class AccountViewModel
{
    public short AccountId { get; set; }

    [Required(ErrorMessage = "Full Name is required")]
    public string? AccountName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; } = 1; // 1: Staff, 2: Lecturer

    public string? AccountPassword { get; set; }

    public string RoleName => AccountRole switch
    {
        0 => "Admin",
        1 => "Staff",
        2 => "Lecturer",
        _ => "User"
    };
}

public class NewsArticleViewModel
{
    public string NewsArticleId { get; set; } = string.Empty;

    [Required(ErrorMessage = "News Title is required")]
    [StringLength(400)]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Headline is required")]
    [StringLength(150)]
    public string Headline { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "News Content is required")]
    public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Please select a Category")]
    public short? CategoryId { get; set; }
    public CategoryViewModel? Category { get; set; }

    public bool? NewsStatus { get; set; } = true;

    public short? CreatedById { get; set; }
    public AccountViewModel? CreatedBy { get; set; }

    public List<int> SelectedTagIds { get; set; } = new();
    public List<TagViewModel> Tags { get; set; } = new();
}

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseModel
{
    public short AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountEmail { get; set; } = string.Empty;
    public int AccountRole { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class ReportViewModel
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TotalArticles { get; set; }
    public List<ReportArticleItem> Articles { get; set; } = new();
}

public class ReportArticleItem
{
    public string NewsArticleId { get; set; } = string.Empty;
    public string? NewsTitle { get; set; }
    public string? Headline { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CategoryName { get; set; }
    public string? CreatedByName { get; set; }
    public bool? NewsStatus { get; set; }
}