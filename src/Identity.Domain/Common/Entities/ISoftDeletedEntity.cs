namespace Identity.Domain.Common.Entities;

public interface ISoftDeletedEntity : IAuditableEntity
{
    bool IsDeleted { get; set; }
    long? DeletedById { get; set; }
    DateTimeOffset? DeletedAt { get; set; }
}
