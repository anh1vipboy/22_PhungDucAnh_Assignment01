using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using _22_PhungDucAnh_Assignment01_BackEnd.Models;
using _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Controllers;

public class SystemAccountsController : ODataController
{
    private readonly ISystemAccountRepository _accountRepo;

    public SystemAccountsController(ISystemAccountRepository accountRepo)
    {
        _accountRepo = accountRepo;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable<SystemAccount> Get()
    {
        return _accountRepo.GetAccounts();
    }

    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] short key)
    {
        var account = await _accountRepo.GetAccountByIdAsync(key);
        if (account == null) return NotFound($"Account with ID {key} not found.");
        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SystemAccount account)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var existing = await _accountRepo.GetAccountByEmailAsync(account.AccountEmail ?? "");
        if (existing != null)
        {
            return BadRequest("An account with this email already exists.");
        }

        var created = await _accountRepo.AddAccountAsync(account);
        return Created(created);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromRoute] short key, [FromBody] SystemAccount account)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        account.AccountId = key;
        var updated = await _accountRepo.UpdateAccountAsync(account);
        if (updated == null) return NotFound($"Account with ID {key} not found.");
        return Updated(updated);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] short key)
    {
        bool hasNews = await _accountRepo.HasNewsArticlesAsync(key);
        if (hasNews)
        {
            return BadRequest("Cannot delete Account because this account has already created News Articles.");
        }

        bool deleted = await _accountRepo.DeleteAccountAsync(key);
        if (!deleted) return NotFound($"Account with ID {key} not found.");
        return NoContent();
    }
}