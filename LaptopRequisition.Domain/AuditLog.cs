using System;
using System.ComponentModel.DataAnnotations;

namespace LaptopRequisition.Domain
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Changes { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}