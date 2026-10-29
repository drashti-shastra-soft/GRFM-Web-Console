using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class RoomWindow : IAuditable
    {
        public int Id { get; set; }
        public int QuotationRoomId { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string CurtainType { get; set; }
        public int AuditId { get; set; }

        public List<QuotationRoom> QuotationRooms { get; set; } = new List<QuotationRoom>();
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
