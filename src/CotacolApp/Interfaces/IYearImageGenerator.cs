using System.Threading.Tasks;
using CotacolApp.Models.CotacolApi;

namespace CotacolApp.Interfaces
{
    public interface IYearImageGenerator
    {
        Task<byte[]> CreateImageAsync(YearReview summary);
    }
}