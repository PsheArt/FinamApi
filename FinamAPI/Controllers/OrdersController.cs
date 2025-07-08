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

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> PlaceOrder([FromBody] OrderRequest request)
        {
            request.ClientId = _settings.ClientId;
            var response = await _orderService.PlaceOrderAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResponse<bool>>> CancelOrder([FromBody] CancelOrderRequest request)
        {
            request.ClientId = _settings.ClientId;
            var response = await _orderService.CancelOrderAsync(request);
            return response.IsSuccess ? Ok(response) : BadRequest(response.Error);
        }


    }
}
