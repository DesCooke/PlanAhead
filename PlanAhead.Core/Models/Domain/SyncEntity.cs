using PlanAhead.Core.Models.Base;

public abstract class SyncEntity : BaseEntity
{
    public Guid UserId { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime UpdatedUtc { get; set; }

    public DateTime? DeletedUtc { get; set; }
}