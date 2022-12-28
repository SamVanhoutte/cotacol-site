using Cotacol.Website.Models.CotacolApi;

namespace Cotacol.Website.Interfaces
{
    public interface IYearImageGenerator
    {
        bool IsExperimental { get; }
        Task<byte[]> CreateImageAsync(YearReview summary);
    }
}