using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Services.Planning
{
    public class LedgerProjectionService : ILedgerProjectionService
    {
        public LedgerProjectionService()
        {
        }

        public IEnumerable<ProjectionEntry> Generate(
            IEnumerable<LedgerEntry> ledgerEntries,
            DateOnly from,
            DateOnly to)
        {
            return ledgerEntries
                .Where(e => e.EntryDate >= from)
                .Where(e => e.EntryDate <= to)
                .OrderBy(e => e.EntryDate)
                .Select(e => new ProjectionEntry
                {
                    AccountId = e.AccountId,
                    FundId = e.FundId,
                    Date = e.EntryDate,
                    Amount = -e.Amount,
                    Type = ProjectionType.Ledger
                });
        }

    }
}
