using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace PeruCompras.IntegracionMef.Transversal.Common.Resources
{
    public static class KeyVault
    {
        public static string ObtenerKeyVault(string Uri, string keyVault)
        {
            var client = new SecretClient(vaultUri: new Uri(Uri), credential: new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret(keyVault);
            return secret.Value;
        }
    }
}