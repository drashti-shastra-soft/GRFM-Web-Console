using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Service
{
    public class ClientService : IClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public ClientService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastucture.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastucture.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastucture.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.ClientLister> GetAllAsync(Domain.ClientLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.ClientLister>(
                url: $"{_baseUrl}/Client/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Client> CreateAsync(Domain.Client mClient)
        {
            return await _httpClientService.PostAsync<Domain.Client, Domain.Client>(
                url: $"{_baseUrl}/Client",
                obj: mClient,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Client> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Client>(
            url: $"{_baseUrl}/Client/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Client/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
