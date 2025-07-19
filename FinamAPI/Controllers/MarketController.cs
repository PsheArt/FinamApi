using FinamAPI.Models;
using FinamAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinamAPI.Controllers
{
   [ApiController]
    [Route("api/market")]
    public class MarketController : ControllerBase
    {
        private readonly IMarketDataService _marketDataService;

        public MarketController(IMarketDataService marketDataService)
        {
            _marketDataService = marketDataService;
        }
        [HttpGet("securities")]
        public async Task<ActionResult<ApiResponse<List<Security>>>> GetSecurities(
           [FromQuery] string? board = null, [FromQuery] string? seccode = null)
        {

            var response = await _marketDataService.GetSecuritiesAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }

        [HttpGet("securities/{code}/properties")]
        public async Task<ActionResult<SecurityProperties>> GetSecurityProperties(string code)
        {
            var response = await _marketDataService.GetSecuritiesAsync();

            if (!response.IsSuccess || response.Data == null || response.Data.Count == 0)
            {
                return NotFound();
            }

            var security = response.Data.First();
            return Ok(security.Properties);
        }

        [HttpGet("candles/{securityCode}")]
        public async Task<ActionResult<ApiResponse<List<Candle>>>> GetCandles(
           string securityCode, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var response = await _marketDataService.GetCandlesAsync(securityCode, from, to, "D1");
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }


    }
}
