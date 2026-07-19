using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Interfaces.Services;

public interface IForecastEngine
{
    Task<IReadOnlyList<ProjectionEntry>> GenerateForecastAsync(
        Guid accountId,
        DateOnly from,
        DateOnly to);
}