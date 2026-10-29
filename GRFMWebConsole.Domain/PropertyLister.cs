using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class PropertyLister
    {
        public List<Property> List { get; set; } = new List<Property>();

        public Property SearchCriteria { get; set; } = new Property();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
