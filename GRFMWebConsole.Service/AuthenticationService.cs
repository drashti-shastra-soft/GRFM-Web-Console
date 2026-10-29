using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _apiBaseUrl;
        public AuthenticationService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _apiBaseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");
        }

        public async Task<Domain.User> AuthenticateAsync(Domain.Credential mCredential)
        {
            return await _httpClientService.PostAsync<Domain.Credential, Domain.User>(
                url: $"{_apiBaseUrl}/Authentication",
                obj: mCredential
            );
        }
    }
}
