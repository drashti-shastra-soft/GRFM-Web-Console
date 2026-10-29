using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Service
{
    public class QuotationService : IQuotationService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public QuotationService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastucture.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastucture.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastucture.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.QuotationLister> GetAllAsync(Domain.QuotationLister mLister)
        {
            var result = await _httpClientService.PostAsync<Domain.QuotationLister>(
                url: $"{_baseUrl}/Quotation/GetAll",
                obj: mLister,
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

        public async Task<Domain.Quotation> CreateAsync(Domain.Quotation mQuotation)
        {
            return await _httpClientService.PostAsync<Domain.Quotation, Domain.Quotation>(
                url: $"{_baseUrl}/Quotation",
                obj: mQuotation,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.Quotation> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.Quotation>(
            url: $"{_baseUrl}/Quotation/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Quotation/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }

        public async Task<string> GenerateQuoteAsync(int id)
        {
            var result = await _httpClientService.GetAsync<string>(
                url: $"{_baseUrl}/Quotation/GenerateQuote/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteFabricRoomsAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/Quotation/DeleteFabricRooms/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }
    }
}
