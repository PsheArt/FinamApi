using FinamAPI.Models;

namespace FinamAPI.Services
{
    public interface IOrderService
    {
        Task<ApiResponse<List<Order>>> GetOrdersAsync(string clientId, bool includeMatched = false,
            bool includeCanceled = false, bool includeActive = true);

        Task<ApiResponse<OrderResponse>> PlaceOrderAsync(OrderRequest request);
        Task<ApiResponse<bool>> CancelOrderAsync(CancelOrderRequest request);
    }
}
