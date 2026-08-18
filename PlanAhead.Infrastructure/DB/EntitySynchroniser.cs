using PlanAhead.Core.Models.Domain;
using PlanAhead.Infrastructure.Authentication;
using PlanAhead.Infrastructure.Repositories;
using Supabase;
using Supabase.Postgrest.Models;

namespace PlanAhead.Infrastructure.DB;

public abstract class EntitySynchroniser<TEntity>
    where TEntity : SyncEntity
{
    protected Client Supabase { get; }

    protected EntitySynchroniser(Client supabase)
    {
        Supabase = supabase;
    }

    public abstract Task UploadAsync(TEntity entity);


    public abstract Task DownloadChangesAsync(
        DateTime sinceUtc,
        CancellationToken cancellationToken = default);

    protected async Task UploadRecordAsync<TRecord>(TRecord record)
        where TRecord : Supabase.Postgrest.Models.BaseModel, new()
    {
        await Supabase
            .From<TRecord>()
            .Upsert(record);
    }


    protected async Task DeleteRecordAsync<TRecord>(TRecord record)
        where TRecord : Supabase.Postgrest.Models.BaseModel, new()
    {
        await Supabase
            .From<TRecord>()
            .Delete(record);
    }

    public abstract Task UploadPendingAsync(
        Guid userId, CancellationToken cancellationToken = default);

}