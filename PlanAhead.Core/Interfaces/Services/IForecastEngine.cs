using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Interfaces.Services;

public interface IForecastEngine
{
    Forecast Generate(
        IEnumerable<FundingRule> fundingRules,
        DateOnly from,
        DateOnly to);
}