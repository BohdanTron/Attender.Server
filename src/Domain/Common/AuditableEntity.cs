using System;

namespace Attender.Server.Domain.Common
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = default!;
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
