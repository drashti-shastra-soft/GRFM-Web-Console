using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IQuotationItemService
    {
        Task<Domain.QuotationItem> CreateAsync(Domain.QuotationItem mQuotationItem);
        
        Task<Domain.QuotationItem> GetAsync(int id);
        
        Task DeleteAsync(int id);        
    }
}
