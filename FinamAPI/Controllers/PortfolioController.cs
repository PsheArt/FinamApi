using FinamAPI.Configs;
using FinamAPI.Models;
using FinamAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FinamAPI.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly FinamApiSettings _settings;

        public PortfolioController(
            IPortfolioService portfolioService,
            IOptions<FinamApiSettings> settings)
        {
            _portfolioService = portfolioService;
            _settings = settings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<Portfolio>>> GetPortfolio(
            [FromQuery] bool includeCurrencies = true,
            [FromQuery] bool includeMoney = true,
            [FromQuery] bool includePositions = true,
            [FromQuery] bool includeMaxBuySell = true)
        {
            var filter = new PortfolioFilter
            {
                ClientId = _settings.ClientId,
                IncludeCurrencies = includeCurrencies,
                IncludeMoney = includeMoney,
                IncludePositions = includePositions,
                IncludeMaxBuySell = includeMaxBuySell
            };

            var response = await _portfolioService.GetPortfolioAsync(filter);
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }

        [HttpGet("positions")]
        public async Task<ActionResult<ApiResponse<List<PositionRow>>>> GetPositions()
        {
            var filter = new PortfolioFilter
            {
                ClientId = _settings.ClientId,
                IncludeCurrencies = false,
                IncludeMoney = false,
                IncludePositions = true,
                IncludeMaxBuySell = true
            };

            var response = await _portfolioService.GetPortfolioAsync(filter);
            return response.IsSuccess
                ? Ok(new ApiResponse<List<PositionRow>> { Data = response.Data?.Positions })
                : BadRequest(response.Error);
        }

        [HttpGet("currencies")]
        public async Task<ActionResult<ApiResponse<List<CurrencyRow>>>> GetCurrencies()
        {
            var filter = new PortfolioFilter
            {
                ClientId = _settings.ClientId,
                IncludeCurrencies = true,
                IncludeMoney = false,
                IncludePositions = false,
                IncludeMaxBuySell = false
            };

            var response = await _portfolioService.GetPortfolioAsync(filter);
            return response.IsSuccess
                ? Ok(new ApiResponse<List<CurrencyRow>> { Data = response.Data?.Currencies })
                : BadRequest(response.Error);
        }
    }
}