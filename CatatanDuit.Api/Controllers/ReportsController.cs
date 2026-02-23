using System.Security.Claims;
using CatatanDuit.Api.Common;
using CatatanDuit.Api.Dtos.Reports;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatatanDuit.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WalletSummaryDto>>>> GetSummary()
    {
        var userId = GetUserId();
        var summary = await _reportService.GetSummaryAsync(userId);
        return Ok(ApiResponse<IEnumerable<WalletSummaryDto>>.Ok(summary));
    }

    [HttpGet("monthly")]
    public async Task<ActionResult<ApiResponse<IEnumerable<MonthlyReportItemDto>>>> GetMonthlyReport([FromQuery] int year)
    {
        var userId = GetUserId();
        var report = await _reportService.GetMonthlyReportAsync(userId, year);
        return Ok(ApiResponse<IEnumerable<MonthlyReportItemDto>>.Ok(report));
    }

    [HttpGet("by-category")]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryReportItemDto>>>> GetByCategory(
        [FromQuery] Guid? walletId = null,
        [FromQuery] int? month = null,
        [FromQuery] int? year = null)
    {
        var userId = GetUserId();
        var report = await _reportService.GetByCategoryAsync(userId, walletId, month, year);
        return Ok(ApiResponse<IEnumerable<CategoryReportItemDto>>.Ok(report));
    }
}
