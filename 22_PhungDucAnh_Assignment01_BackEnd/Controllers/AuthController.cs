using Microsoft.AspNetCore.Mvc;
using _22_PhungDucAnh_Assignment01_BackEnd.DTOs;
using _22_PhungDucAnh_Assignment01_BackEnd.Repositories;

namespace _22_PhungDucAnh_Assignment01_BackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ISystemAccountRepository _accountRepo;

    public AuthController(IConfiguration config, ISystemAccountRepository accountRepo)
    {
        _config = config;
        _accountRepo = accountRepo;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Email and Password are required.");
        }

        // 1. Kiá»ƒm tra tÃ i khoáº£n Admin tá»« appsettings.json
        var adminEmail = _config["AdminAccount:Email"];
        var adminPassword = _config["AdminAccount:Password"];

        if (string.Equals(request.Email, adminEmail, StringComparison.OrdinalIgnoreCase) &&
            request.Password == adminPassword)
        {
            return Ok(new LoginResponse
            {
                AccountId = 0,
                AccountName = "Administrator",
                AccountEmail = adminEmail!,
                AccountRole = 0, // Admin Role
                RoleName = "Admin"
            });
        }

        // 2. Kiá»ƒm tra tÃ i khoáº£n trong Database
        var account = await _accountRepo.AuthenticateAsync(request.Email, request.Password);
        if (account == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        string roleName = account.AccountRole switch
        {
            1 => "Staff",
            2 => "Lecturer",
            _ => "User"
        };

        return Ok(new LoginResponse
        {
            AccountId = account.AccountId,
            AccountName = account.AccountName ?? "Staff Member",
            AccountEmail = account.AccountEmail ?? request.Email,
            AccountRole = account.AccountRole ?? 1,
            RoleName = roleName
        });
    }
}