using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class Product : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategory { get; set; }
        public int WidthOfRoll { get; set; }
        public string Status { get; set; }
        public bool IsReadymade { get; set; }
        public bool IsActive { get; set; } = true;
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
