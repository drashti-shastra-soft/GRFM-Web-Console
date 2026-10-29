using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IQuotationService
    {
        Task<Domain.QuotationLister> GetAllAsync(Domain.QuotationLister mLister);
        
        Task<Domain.Quotation> CreateAsync(Domain.Quotation mQuotation);
        
        Task<Domain.Quotation> GetAsync(int id);
        
        Task DeleteAsync(int id);
        
        Task<string> GenerateQuoteAsync(int id);
        
        Task DeleteFabricRoomsAsync(int id);
    }
}
