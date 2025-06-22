using FinamAPI.Models;

namespace FinamAPI.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<ApiResponse<Portfolio>> GetPortfolioAsync(PortfolioFilter filter);
    }
}