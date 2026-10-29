using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRFMWebConsole.Domain
{
    public class AuthVerificationLog : IAuditable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OTP { get; set; }
        public bool IsVerified { get; set; }
        public int AuditId { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
    }
}
