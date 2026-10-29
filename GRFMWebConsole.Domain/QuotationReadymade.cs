using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class QuotationReadymade : IAuditable
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public int UnitOfMeasureId { get; set; }
        public decimal MRP { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalRate { get; set; }
        public decimal Amount { get; set; }
        public int AuditId { get; set; }

        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
