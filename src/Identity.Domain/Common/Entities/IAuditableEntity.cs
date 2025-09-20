namespace Identity.Domain.Common.Entities;

public interface IAuditableEntity : IEntity
{
    long CreatedById { get; set; }
    DateTimeOffset CreatedAt { get; set; }

    long? ModifiedById { get; set; }
    DateTimeOffset? ModifiedAt { get; set; }
}
