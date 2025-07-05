using FinamAPI.Configs;
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

    }
}
