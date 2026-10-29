using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Infrastucture.IService
{
    public interface IDropdownService
    {
        dynamic GetStatusOptions();

        dynamic GetPageSizeOptions(int selectedSize = 20);

        Task<dynamic> GetUserTypeOptionsAsync();

        Task<dynamic> GetUnitOfMeasureOptionsAsync();

        Task<dynamic> GetClientOptionsAsync();
    }
}
