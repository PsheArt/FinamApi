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
           [FromQuery] string board = null, [FromQuery] string seccode = null)
        {

            var response = await _marketDataService.GetSecuritiesAsync();
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }


    }
}
