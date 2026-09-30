using _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

public interface ISystemAccountRepository
{
    IQueryable<SystemAccount> GetAccounts();
    Task<SystemAccount?> GetAccountByIdAsync(short id);
    Task<SystemAccount?> GetAccountByEmailAsync(string email);
    Task<SystemAccount> AddAccountAsync(SystemAccount account);
    Task<SystemAccount?> UpdateAccountAsync(SystemAccount account);
    Task<bool> DeleteAccountAsync(short id);
    Task<bool> HasNewsArticlesAsync(short accountId);
    Task<SystemAccount?> AuthenticateAsync(string email, string password);
}

public class SystemAccountRepository : ISystemAccountRepository
{
    public IQueryable<SystemAccount> GetAccounts() => SystemAccountDAO.Instance.GetAccounts();
    public Task<SystemAccount?> GetAccountByIdAsync(short id) => SystemAccountDAO.Instance.GetAccountByIdAsync(id);
    public Task<SystemAccount?> GetAccountByEmailAsync(string email) => SystemAccountDAO.Instance.GetAccountByEmailAsync(email);
    public Task<SystemAccount> AddAccountAsync(SystemAccount account) => SystemAccountDAO.Instance.AddAccountAsync(account);
    public Task<SystemAccount?> UpdateAccountAsync(SystemAccount account) => SystemAccountDAO.Instance.UpdateAccountAsync(account);
    public Task<bool> DeleteAccountAsync(short id) => SystemAccountDAO.Instance.DeleteAccountAsync(id);
    public Task<bool> HasNewsArticlesAsync(short accountId) => SystemAccountDAO.Instance.HasNewsArticlesAsync(accountId);
    public Task<SystemAccount?> AuthenticateAsync(string email, string password) => SystemAccountDAO.Instance.AuthenticateAsync(email, password);
}