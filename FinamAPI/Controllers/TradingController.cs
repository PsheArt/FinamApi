using FinamAPI.Models;
using FinamAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinamAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TradingController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TradingController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Index() => Ok("Trading service is running");

        [HttpGet("token")]   
        public async Task<IActionResult> GetToken()
        {
            try
            {
                var response = await _authService.AuthenticateAsync();

                if (!response.IsSuccess)
                    return BadRequest("Authentication failed"); 

                return Ok(new { Token = response.AccessToken }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}