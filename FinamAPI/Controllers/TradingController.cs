using FinamAPI.Models;
using FinamAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinamAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradingController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TradingController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("token")]   
        public async Task<IActionResult> GetToken()
        {
            try
            {
                var response = await _authService.AuthenticateAsync();
                return Ok(new
                {
                    Token = response.AccessToken,
                    ExpiresIn = response.ExpiresIn,
                    ExpiresAt = response.TokenExpiration
                });
            }
            catch (ApiException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
        }
    }
}