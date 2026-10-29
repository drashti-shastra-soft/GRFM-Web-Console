using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IQuotationPDFTitleService
    {
        Task<Domain.QuotationPDFTitle> CreateAsync(Domain.QuotationPDFTitle mQuotationPDFTitle);
        Task<Domain.QuotationPDFTitle> GetAsync(int id);
        Task DeleteAsync(int id);
        Task<Domain.QuotationPDFTitle> GetPDFTitle(int id);
    }
}
