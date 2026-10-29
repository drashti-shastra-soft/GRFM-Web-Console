using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class Property : IAuditable
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Client { get; set; }
        public string PropertyNumber { get; set; }
        public string SocietyName { get; set; } 
        public string LandMark { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string State { get; set; }   
        public string ZipCode { get; set; }
        public string FullAddress { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
