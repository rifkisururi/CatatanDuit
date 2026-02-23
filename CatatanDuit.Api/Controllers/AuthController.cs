using System.Security.Claims;
using CatatanDuit.Api.Common;
using CatatanDuit.Api.Dtos.Auth;
using CatatanDuit.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatatanDuit.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto dto)
    {
        try
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Registration successful"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<AuthResponseDto>.Fail(ex.Message));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto dto)
    {
        try
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(ApiResponse<AuthResponseDto>.Ok(result, "Login successful"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.Fail(ex.Message));
        }
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<ApiResponse<object>> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(ApiResponse<object>.Ok(new
        {
            Id = Guid.Parse(userId!),
            Email = email
        }));
    }
}
