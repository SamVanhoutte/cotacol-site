namespace Cotacol.Website.Interfaces
{
    public interface ISecretReader
    {
        Task<string> ReadSecret(string secretName);
    }
}