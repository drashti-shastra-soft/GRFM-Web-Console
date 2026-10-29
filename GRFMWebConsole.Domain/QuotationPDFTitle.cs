using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class QuotationPDFTitle : IAuditable
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public string FabricPDFTitle { get; set; }
        public string LabourPDFTitle { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        public decimal FabricGrandTotal { get; set; }
        public decimal LabourGrandTotal { get; set; }
    }
}
