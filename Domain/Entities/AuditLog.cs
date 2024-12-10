using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string OperationType { get; set; }
        public string? PrimaryKeyValue { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime ChangeDateTime { get; set; }
        public string? ChangedBy { get; set; }

        public AuditLog()
        {
        }
    }
}
