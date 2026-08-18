using SQLite;

namespace PlanAhead.Core.Models.Base;

public abstract class BaseEntity
{
    [PrimaryKey]
    public Guid Id { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime UpdatedUtc { get; set; }

    public bool Deleted { get; set; }
    public DateTime DeletedUtc { get; set; }

    public bool NeedsSync { get; set; }
}