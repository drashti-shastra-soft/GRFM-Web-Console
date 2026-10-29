using GRFMWebConsole.Domain;
using GRFMWebConsole.Infrastucture.IService;
using System.Configuration;
using System.Threading.Tasks;

namespace GRFMWebConsole.Service
{
    public class QuotationPDFTitleService : IQuotationPDFTitleService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public QuotationPDFTitleService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastucture.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastucture.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastucture.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.QuotationPDFTitle> CreateAsync(Domain.QuotationPDFTitle mQuotationPDFTitle)
        {
            return await _httpClientService.PostAsync<Domain.QuotationPDFTitle, Domain.QuotationPDFTitle>(
                url: $"{_baseUrl}/QuotationPDFTitle",
                obj: mQuotationPDFTitle,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task<Domain.QuotationPDFTitle> GetAsync(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.QuotationPDFTitle>(
            url: $"{_baseUrl}/QuotationPDFTitle/{id}",
                token: _token
            ).ConfigureAwait(false);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await _httpClientService.DeleteAsync(
                url: $"{_baseUrl}/QuotationPDFTitle/{id}/LoginUser/{_loginUserId}",
                token: _token
             ).ConfigureAwait(false);
        }

        public async Task<Domain.QuotationPDFTitle> GetPDFTitle(int id)
        {
            var result = await _httpClientService.GetAsync<Domain.QuotationPDFTitle>(
                url: $"{_baseUrl}/QuotationPDFTitle/QuotationId/{id}",
                token: _token
            ).ConfigureAwait(false);

            return result;
        }

    }
}
