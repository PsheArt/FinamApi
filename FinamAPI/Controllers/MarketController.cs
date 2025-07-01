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
    }
}
