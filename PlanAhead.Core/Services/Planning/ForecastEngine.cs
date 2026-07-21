using PlanAhead.Core.Interfaces.Repositories;
using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Services.Planning;

public class ForecastEngine : IForecastEngine
{
    private readonly IFundRepository _fundRepository;
    private readonly IFundingRuleRepository _fundingRuleRepository;
    private readonly ILedgerEntryRepository _ledgerEntryRepository;

    private readonly IFundingProjectionService _fundingProjectionService;
    private readonly ILedgerProjectionService _ledgerProjectionService;

    public ForecastEngine(
        IFundRepository fundRepository,
        IFundingRuleRepository fundingRuleRepository,
        ILedgerEntryRepository ledgerEntryRepository,
        IFundingProjectionService fundingProjectionService,
        ILedgerProjectionService ledgerProjectionService)
    {
        _fundRepository = fundRepository;
        _fundingRuleRepository = fundingRuleRepository;
        _ledgerEntryRepository = ledgerEntryRepository;

        _fundingProjectionService = fundingProjectionService;
        _ledgerProjectionService = ledgerProjectionService;
    }

    public async Task<Forecast> GenerateAsync(
        Guid accountId,
        DateOnly from,
        DateOnly to)
    {
        var forecast = new Forecast
        {
            From = from,
            To = to
        };

        //
        // Funding projections
        //

        var funds =
            await _fundRepository.GetByAccountIdAsync(accountId);

        foreach (var fund in funds)
        {
            var rules =
                await _fundingRuleRepository
                    .GetByFundIdAsync(fund.Id);

            forecast.Entries.AddRange(
                _fundingProjectionService.Generate(
                    fund,
                    rules,
                    from,
                    to));
        }

        //
        // Ledger projections
        //

        var ledgerEntries =
            await _ledgerEntryRepository
                .GetByAccountIdAsync(accountId);

        forecast.Entries.AddRange(
            _ledgerProjectionService.Generate(
                ledgerEntries,
                from,
                to));

        //
        // Final ordering
        //

        forecast.Entries = forecast.Entries
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Type)
            .ToList();

        return forecast;
    }
}