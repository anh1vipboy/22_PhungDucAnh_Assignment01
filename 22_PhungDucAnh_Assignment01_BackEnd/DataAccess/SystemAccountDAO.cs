using Microsoft.EntityFrameworkCore;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;

namespace _22_PhungDucAnh_Assignment01_BackEnd.DataAccess;

public class SystemAccountDAO
{
    private static SystemAccountDAO? _instance;
    private static readonly object _instanceLock = new();

    private SystemAccountDAO() { }

    public static SystemAccountDAO Instance
    {
        get
        {
            lock (_instanceLock)
            {
                _instance ??= new SystemAccountDAO();
                return _instance;
            }
        }
    }

    public IQueryable<SystemAccount> GetAccounts()
    {
        var context = new FUNewsManagementContext();
        return context.SystemAccounts.AsQueryable();
    }

    public async Task<SystemAccount?> GetAccountByIdAsync(short id)
    {
        using var context = new FUNewsManagementContext();
        return await context.SystemAccounts.FindAsync(id);
    }

    public async Task<SystemAccount?> GetAccountByEmailAsync(string email)
    {
        using var context = new FUNewsManagementContext();
        return await context.SystemAccounts
            .FirstOrDefaultAsync(a => a.AccountEmail != null && a.AccountEmail.ToLower() == email.ToLower());
    }

    public async Task<SystemAccount> AddAccountAsync(SystemAccount account)
    {
        using var context = new FUNewsManagementContext();
        if (account.AccountId == 0)
        {
            short maxId = await context.SystemAccounts.AnyAsync() 
                ? await context.SystemAccounts.MaxAsync(a => a.AccountId) 
                : (short)0;
            account.AccountId = (short)(maxId + 1);
        }
        context.SystemAccounts.Add(account);
        await context.SaveChangesAsync();
        return account;
    }

    public async Task<SystemAccount?> UpdateAccountAsync(SystemAccount account)
    {
        using var context = new FUNewsManagementContext();
        var existing = await context.SystemAccounts.FindAsync(account.AccountId);
        if (existing == null) return null;

        existing.AccountName = account.AccountName;
        existing.AccountEmail = account.AccountEmail;
        existing.AccountRole = account.AccountRole;
        if (!string.IsNullOrWhiteSpace(account.AccountPassword))
        {
            existing.AccountPassword = account.AccountPassword;
        }

        await context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAccountAsync(short id)
    {
        using var context = new FUNewsManagementContext();
        var account = await context.SystemAccounts.FindAsync(id);
        if (account == null) return false;

        context.SystemAccounts.Remove(account);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasNewsArticlesAsync(short accountId)
    {
        using var context = new FUNewsManagementContext();
        return await context.NewsArticles.AnyAsync(n => n.CreatedById == accountId);
    }

    public async Task<SystemAccount?> AuthenticateAsync(string email, string password)
    {
        using var context = new FUNewsManagementContext();
        return await context.SystemAccounts
            .FirstOrDefaultAsync(a => a.AccountEmail == email && a.AccountPassword == password);
    }
}