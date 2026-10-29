using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IQuotationLabourService
    {
        Task<Domain.QuotationLabour> CreateAsync(Domain.QuotationLabour mQuotationLabour);
        Task<Domain.QuotationLabour> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
