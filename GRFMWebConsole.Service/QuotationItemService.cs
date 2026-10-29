using GRFMWebConsole.Domain;
using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Service
{
    public class QuotationItemService : IQuotationItemService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public QuotationItemService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastucture.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastucture.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastucture.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.QuotationItem> CreateAsync(Domain.QuotationItem mQuotationItem)
        {
            return await _httpClientService.PostAsync<Domain.QuotationItem, Domain.QuotationItem>(
                url: $"{_baseUrl}/QuotationItem",
                obj: mQuotationItem,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.QuotationItem> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.QuotationItem>(
            url: $"{_baseUrl}/QuotationItem/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/QuotationItem/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
