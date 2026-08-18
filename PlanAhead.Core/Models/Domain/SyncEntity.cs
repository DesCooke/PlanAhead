using PlanAhead.Core.Models.Base;
using SQLite;

public abstract class SyncEntity : BaseEntity
{
    [Indexed]
    public Guid UserId { get; set; }
}