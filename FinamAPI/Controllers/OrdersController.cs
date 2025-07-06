using FinamAPI.Configs;
using FinamAPI.Models;
using FinamAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FinamAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly FinamApiSettings _settings;

        public OrdersController(
            IOrderService orderService,
            IOptions<FinamApiSettings> settings)
        {
            _orderService = orderService;
            _settings = settings.Value;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Order>>>> GetOrders(
           [FromQuery] bool includeMatched = false,
           [FromQuery] bool includeCanceled = false,
           [FromQuery] bool includeActive = true)
        {
            var response = await _orderService.GetOrdersAsync(
                _settings.ClientId, includeMatched, includeCanceled, includeActive);

            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }



    }
}
