using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class FabricAccessory : IAuditable
    {
        public int Id { get; set; }
        public int RoomWindowId { get; set; }
        public string Description { get; set; }
        public decimal Qty { get; set; }
        public int UnitOfMeasureId { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public List<RoomWindow> RoomWindows { get; set; } = new List<RoomWindow>();
    }
}
