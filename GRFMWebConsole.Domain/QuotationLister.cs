using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class QuotationLister
    {
        public List<Quotation> List { get; set; } = new List<Quotation>();

        public Quotation SearchCriteria { get; set; } = new Quotation();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
