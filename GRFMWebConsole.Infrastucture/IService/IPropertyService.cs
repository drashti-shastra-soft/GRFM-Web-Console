using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IPropertyService
    {
        Task<Domain.PropertyLister> GetAllAsync(Domain.PropertyLister mLister);
        Task<Domain.Property> CreateAsync(Domain.Property mProperty);
        Task<Domain.Property> GetAsync(int id); 
        Task DeleteAsync(int id);
    }
}
