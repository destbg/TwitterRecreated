using System;

namespace Domain.Common
{
    public class AuditableEntity
    {
        public string CreatedByIp { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
