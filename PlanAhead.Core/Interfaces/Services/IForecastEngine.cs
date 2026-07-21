using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Interfaces.Services;

public interface IForecastEngine
{
    Task<Forecast> GenerateAsync(
        Guid accountId,
        DateOnly from,
        DateOnly to);
}