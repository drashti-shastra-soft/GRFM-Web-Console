using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Service
{
    public class PropertyService : IPropertyService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public PropertyService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastucture.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastucture.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastucture.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.PropertyLister> GetAllAsync(Domain.PropertyLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.PropertyLister>(
                url: $"{_baseUrl}/Property/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Property> CreateAsync(Domain.Property mProperty)
        {
            return await _httpClientService.PostAsync<Domain.Property, Domain.Property>(
                url: $"{_baseUrl}/Property",
                obj: mProperty,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Property> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Property>(
            url: $"{_baseUrl}/Property/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Property/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
