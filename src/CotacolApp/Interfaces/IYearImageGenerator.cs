using System.Threading.Tasks;
using CotacolApp.Models.CotacolApi;

namespace CotacolApp.Interfaces
{
    public interface IYearImageGenerator
    {
        bool IsExperimental { get; }
        Task<byte[]> CreateImageAsync(YearReview summary);
    }
}