using GRFMWebConsole.Infrastucture.IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Service
{
    public class AuthVerificationLogService : IAuthVerificationLogService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly string _baseUrl;
        private readonly string _token;
        private readonly int _loginUserId;

        public AuthVerificationLogService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
            _baseUrl = ConfigurationManager.AppSettings.Get("APIBaseUrl");

            if (Infrastucture.HttpContextHelper.LoginUser != null)
            {
                _token = Infrastucture.HttpContextHelper.LoginUser.Token;
                _loginUserId = Infrastucture.HttpContextHelper.LoginUser.Id;
            }

        }

        public async Task<Domain.User> VerifyOTPAsync(Domain.AuthVerificationLog mAuthVerificationLog)
        {
            return await _httpClientService.PostAsync<Domain.AuthVerificationLog, Domain.User>(
                url: $"{_baseUrl}/AuthVerificationLog/VerifyOTP",
                obj: mAuthVerificationLog,
                token: _token
            ).ConfigureAwait(false);
        }

        public async Task ResendOTPAsync(Domain.AuthVerificationLog mAuthVerificationLog)
        {
            await _httpClientService.PostAsync<Domain.AuthVerificationLog, object>(
                    url: $"{_baseUrl}/AuthVerificationLog/ResendOTP",
                    obj: mAuthVerificationLog,
                    token: _token
            ).ConfigureAwait(false);
        }
    }
}
