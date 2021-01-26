using System.Threading.Tasks;

namespace CotacolApp.Interfaces
{
    public interface ISecretReader
    {
        Task<string> ReadSecret(string secretName);
    }
}