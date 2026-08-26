using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace PlanAhead.Infrastructure.Sync.Models;

[Table("user_sync")]
public class UserSyncRecord : BaseModel
{
    [PrimaryKey("user_id")]
    public Guid UserId { get; set; }

    [Column("sync_version")]
    public long SyncVersion { get; set; }

    [Column("last_updated_utc")]
    public DateTime LastUpdatedUtc { get; set; }
}