using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class QuotationRoom : IAuditable
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public string RoomName { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public List<QuotationItem> QuotationItems { get; set; } = new List<QuotationItem>();
        public List<QuotationLabour> QuotationLabours { get; set; } = new List<QuotationLabour>();
    }
}
