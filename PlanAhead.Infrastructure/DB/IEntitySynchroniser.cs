namespace PlanAhead.Infrastructure.DB
{

    public interface IEntitySynchroniser
    {
        string EntityName { get; }

        Task UploadPendingAsync(Guid userId, CancellationToken cancellationToken = default);

        Task DownloadChangesAsync(
            DateTime sinceUtc,
            CancellationToken cancellationToken = default);
    }
}