namespace Identity.Persistence.UnitOfWorks.Interfaces;

public interface IUserContext
{
    long SystemId { get; }

    long? UserId { get; }

    long GetCurrentUserId();
}
