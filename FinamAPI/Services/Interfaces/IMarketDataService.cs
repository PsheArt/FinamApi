using FinamAPI.Models;

namespace FinamAPI.Services.Interfaces
{
    public interface IMarketDataService
    {
        Task<ApiResponse<List<Security>>> GetSecuritiesAsync();
        Task<ApiResponse<List<Candle>>> GetCandlesAsync(string securityCode, DateTime from, DateTime to, string timeframe);
    }
