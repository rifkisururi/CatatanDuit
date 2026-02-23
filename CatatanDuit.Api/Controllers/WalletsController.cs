using System.Security.Claims;
using CatatanDuit.Api.Common;
using CatatanDuit.Api.Dtos.Wallets;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatatanDuit.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class WalletsController : ControllerBase
{
    private readonly IWalletService _walletService;

    public WalletsController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<WalletDto>>>> GetWallets()
    {
        var userId = GetUserId();
        var wallets = await _walletService.GetWalletsAsync(userId);
        return Ok(ApiResponse<IEnumerable<WalletDto>>.Ok(wallets));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<WalletDto>>> GetWallet(Guid id)
    {
        var userId = GetUserId();
        var wallet = await _walletService.GetWalletByIdAsync(userId, id);

        if (wallet == null)
        {
            return NotFound(ApiResponse<WalletDto>.Fail("Wallet not found"));
        }

        return Ok(ApiResponse<WalletDto>.Ok(wallet));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<WalletDto>>> CreateWallet([FromBody] CreateWalletRequestDto dto)
    {
        var userId = GetUserId();
        var wallet = await _walletService.CreateWalletAsync(userId, dto);
        return CreatedAtAction(nameof(GetWallet), new { id = wallet.Id }, ApiResponse<WalletDto>.Ok(wallet, "Wallet created"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<WalletDto>>> UpdateWallet(Guid id, [FromBody] UpdateWalletRequestDto dto)
    {
        var userId = GetUserId();
        var wallet = await _walletService.UpdateWalletAsync(userId, id, dto);

        if (wallet == null)
        {
            return NotFound(ApiResponse<WalletDto>.Fail("Wallet not found"));
        }

        return Ok(ApiResponse<WalletDto>.Ok(wallet, "Wallet updated"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteWallet(Guid id)
    {
        var userId = GetUserId();
        var success = await _walletService.DeleteWalletAsync(userId, id);

        if (!success)
        {
            return NotFound(ApiResponse<bool>.Fail("Wallet not found"));
        }

        return Ok(ApiResponse<bool>.Ok(true, "Wallet deleted"));
    }
}
