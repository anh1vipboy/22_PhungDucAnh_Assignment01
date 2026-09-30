using _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

public interface ITagRepository
{
    IQueryable<Tag> GetTags();
    Task<Tag?> GetTagByIdAsync(int id);
}

public class TagRepository : ITagRepository
{
    public IQueryable<Tag> GetTags() => TagDAO.Instance.GetTags();
    public Task<Tag?> GetTagByIdAsync(int id) => TagDAO.Instance.GetTagByIdAsync(id);
}