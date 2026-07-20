using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Services.Planning;

public class ForecastEngine : IForecastEngine
{
    private readonly IFundingProjectionService _projectionService;

    public ForecastEngine(
        IFundingProjectionService projectionService)
    {
        _projectionService = projectionService;
    }

    public Forecast Generate(
        IEnumerable<FundingRule> fundingRules,
        DateOnly from,
        DateOnly to)
    {
        var forecast = new Forecast
        {
            From = from,
            To = to
        };

        foreach (var rule in fundingRules)
        {
            forecast.Entries.AddRange(
                _projectionService.Generate(rule, from, to));
        }

        forecast.Entries.Sort((x, y) => x.Date.CompareTo(y.Date));

        return forecast;
    }
}