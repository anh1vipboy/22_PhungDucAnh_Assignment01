using Microsoft.EntityFrameworkCore;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;

public class TagDAO
{
    private static TagDAO? _instance;
    private static readonly object _instanceLock = new();

    private TagDAO() { }

    public static TagDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                _instance ??= new TagDAO();
                return _instance;
            }
        }
    }

    public IQueryable<Tag> GetTags()
    {
        var context = new FUNewsManagementContext();
        return context.Tags.AsQueryable();
    }

    public async Task<Tag?> GetTagByIdAsync(int id)
    {
        using var context = new FUNewsManagementContext();
        return await context.Tags.FindAsync(id);
    }
}