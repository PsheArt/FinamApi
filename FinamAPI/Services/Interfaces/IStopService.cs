using FinamAPI.Models;

namespace FinamAPI.Services.Interfaces
{
    public interface IStopService
    {
        Task<ApiResponse<List<Stop>>> GetStopsAsync(string clientId, bool includeExecuted = false,
            bool includeCanceled = false, bool includeActive = true);

        Task<ApiResponse<StopResponse>> PlaceStopOrderAsync(StopRequest request);
        Task<ApiResponse<bool>> CancelStopOrderAsync(CancelStopRequest request);
    }
}
