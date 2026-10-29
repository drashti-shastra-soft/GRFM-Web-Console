using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IAuthVerificationLogService
    {
        Task<Domain.User> VerifyOTPAsync(Domain.AuthVerificationLog mAuthVerificationLog);

        Task ResendOTPAsync(Domain.AuthVerificationLog mAuthVerificationLog);
    }
}
