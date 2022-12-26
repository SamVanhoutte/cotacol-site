using System.Threading.Tasks;
using Cotacol.Website.Interfaces;
using Cotacol.Website.Models.Settings;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;

namespace Cotacol.Website.Services
{
    public class KeyVaultReader : ISecretReader
    {
        private KeyVaultSettings _settings;

        public KeyVaultReader(IOptions<KeyVaultSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<string> ReadSecret(string secretName)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            var keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secretKeyAddress =
                $"https://{_settings.KeyVaultName}.vault.azure.net/secrets/{secretName.ToLower()}";
            var keyValue =
                await keyVaultClient.GetSecretAsync(secretKeyAddress).ConfigureAwait(false);
            return keyValue.Value;
        }
    }
}