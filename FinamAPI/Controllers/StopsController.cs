using FinamAPI.Configs;
using FinamAPI.Models;
using FinamAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FinamAPI.Controllers
{
    [ApiController]
    [Route("api/stops")]
    public class StopsController : ControllerBase
    {
        private readonly IStopService _stopService;
        private readonly FinamApiSettings _settings;

        public StopsController(
            IStopService stopService,
            IOptions<FinamApiSettings> settings)
        {
            _stopService = stopService;
            _settings = settings.Value;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Stop>>>> GetStops(
            [FromQuery] bool includeExecuted = false,
            [FromQuery] bool includeCanceled = false,
            [FromQuery] bool includeActive = true)
        {
            var response = await _stopService.GetStopsAsync(
                _settings.ClientId, includeExecuted, includeCanceled, includeActive);

            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<StopResponse>>> PlaceStopOrder([FromBody] StopRequest request)
        {
            request.ClientId = _settings.ClientId;
            var response = await _stopService.PlaceStopOrderAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> CancelStopOrder([FromBody] CancelStopRequest request)
        {
            request.ClientId = _settings.ClientId;
            var response = await _stopService.CancelStopOrderAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }
    }
}