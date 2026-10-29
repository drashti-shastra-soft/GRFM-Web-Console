using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IProductService
    {
        Task<Domain.ProductLister> GetAllAsync(Domain.ProductLister mLister);
        Task<Domain.Product> CreateAsync(Domain.Product mProduct);
        Task<Domain.Product> GetAsync(int id);
        Task DeleteAsync(int id);
        Task<bool> CheckProductExistAsync(Domain.Product mProduct);
    }
}
