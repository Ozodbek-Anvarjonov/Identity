namespace Identity.Domain.Common.Entities;

public class SoftDeletedEntity : AuditableEntity, ISoftDeletedEntity
{
    public bool IsDeleted { get; set; }
    public long? DeletedById { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
