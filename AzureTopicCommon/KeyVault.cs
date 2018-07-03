using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
namespace AzureTopicCommon
{
  public  class KeyVault
    {

       string Clientid = string.Empty;
        string ApplicationSecret = string.Empty;
        string ScretIdenfier = string.Empty;
        public KeyVault(string clientid, string applicationSecret, string scretIdenfier)
        {
            Clientid = clientid;
            ApplicationSecret = applicationSecret;
            ScretIdenfier = scretIdenfier;
        }
        public async Task<string> GetKeyValue()
        {
            var keyClient = new KeyVaultClient(authenticationContext);
            var result = await keyClient.GetSecretAsync(ScretIdenfier);
            return result.Value;
        }

        private async Task<string> authenticationContext(string authority, string resource, string scope)
        {
            var adCredential = new ClientCredential(Clientid, ApplicationSecret);
            var authenticationContext = new AuthenticationContext(authority);
            var result = await authenticationContext.AcquireTokenAsync(resource, adCredential);
            return result.AccessToken;
        }
    }
}
