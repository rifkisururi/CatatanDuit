using System.Security.Claims;
using CatatanDuit.Api.Common;
using CatatanDuit.Api.Dtos.Transactions;
using CatatanDuit.Api.Models;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatatanDuit.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<TransactionDto>>>> GetTransactions(
        [FromQuery] Guid? walletId = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] TransactionType? type = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = GetUserId();
        var transactions = await _transactionService.GetTransactionsAsync(
            userId, walletId, categoryId, type, dateFrom, dateTo, page, pageSize);

        return Ok(ApiResponse<PagedResult<TransactionDto>>.Ok(transactions));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> GetTransaction(Guid id)
    {
        var userId = GetUserId();
        var transaction = await _transactionService.GetTransactionByIdAsync(userId, id);

        if (transaction == null)
        {
            return NotFound(ApiResponse<TransactionDto>.Fail("Transaction not found"));
        }

        return Ok(ApiResponse<TransactionDto>.Ok(transaction));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> CreateTransaction([FromBody] CreateTransactionRequestDto dto)
    {
        var userId = GetUserId();

        try
        {
            var transaction = await _transactionService.CreateTransactionAsync(userId, dto);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, ApiResponse<TransactionDto>.Ok(transaction, "Transaction created"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TransactionDto>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TransactionDto>>> UpdateTransaction(Guid id, [FromBody] UpdateTransactionRequestDto dto)
    {
        var userId = GetUserId();

        try
        {
            var transaction = await _transactionService.UpdateTransactionAsync(userId, id, dto);

            if (transaction == null)
            {
                return NotFound(ApiResponse<TransactionDto>.Fail("Transaction not found"));
            }

            return Ok(ApiResponse<TransactionDto>.Ok(transaction, "Transaction updated"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TransactionDto>.Fail(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteTransaction(Guid id)
    {
        var userId = GetUserId();
        var success = await _transactionService.DeleteTransactionAsync(userId, id);

        if (!success)
        {
            return NotFound(ApiResponse<bool>.Fail("Transaction not found"));
        }

        return Ok(ApiResponse<bool>.Ok(true, "Transaction deleted"));
    }
}
