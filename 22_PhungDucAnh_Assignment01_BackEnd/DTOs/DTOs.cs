namespace _22_PhungDucAnh_Assignment01_BackEnd.DTOs;

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public short AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public string AccountEmail { get; set; } = string.Empty;
    public int AccountRole { get; set; } // 0: Admin, 1: Staff, 2: Lecturer
    public string RoleName { get; set; } = string.Empty;
}

public class NewsArticleRequest
{
    public string? NewsArticleId { get; set; }
    public string? NewsTitle { get; set; }
    public string Headline { get; set; } = string.Empty;
    public string? NewsContent { get; set; }
    public string? NewsSource { get; set; }
    public short? CategoryId { get; set; }
    public bool? NewsStatus { get; set; }
    public short? CreatedById { get; set; }
    public short? UpdatedById { get; set; }
    public List<int> TagIds { get; set; } = new();
}

public class AccountRequest
{
    public short AccountId { get; set; }
    public string? AccountName { get; set; }
    public string? AccountEmail { get; set; }
    public int? AccountRole { get; set; }
    public string? AccountPassword { get; set; }
}

public class CategoryRequest
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryDesciption { get; set; } = string.Empty;
    public short? ParentCategoryId { get; set; }
    public bool? IsActive { get; set; }
}