namespace Identity.Domain.Common.Entities;

public class AuditableEntity : Entity, IAuditableEntity
{
    public long CreatedById { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public long? ModifiedById { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}
