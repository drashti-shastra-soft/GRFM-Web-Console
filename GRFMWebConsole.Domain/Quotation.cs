 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class Quotation : IAuditable
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Client { get; set; }
        public string Property { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PropertyId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Discount { get; set; } = 0.0M;
        public decimal FabricGrandTotal { get; set; }
        public decimal LabourGrandTotal { get; set; }
        public decimal? TransportCharges { get; set; } = 0.0M;
        public decimal Amount { get; set; }
        public int AuditId { get; set; }
        public bool Accessory { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }

        public List<QuotationRoom> QuotationRooms { get; set; } = new List<QuotationRoom>();

        //client
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        //property
        public string PropertyNumber { get; set; }
        public string SocietyName { get; set; }
        public string LandMark { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
