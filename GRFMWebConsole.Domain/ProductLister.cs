using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class ProductLister
    {
        public List<Product> List { get; set; } = new List<Product>();

        public Product SearchCriteria { get; set; } = new Product();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
