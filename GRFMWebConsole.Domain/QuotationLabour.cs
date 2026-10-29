using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class QuotationLabour : IAuditable
    {
        public int Id { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasure { get; set; }
        public int QuotationRoomId { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public decimal FinalRate { get; set; }
        public decimal Amount { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
