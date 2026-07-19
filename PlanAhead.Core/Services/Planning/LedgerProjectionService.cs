using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Services.Planning
{
    public class LedgerProjectionService : ILedgerProjectionService
    {
        private readonly IDateCalculator _dateCalculator;

        public LedgerProjectionService(
            IDateCalculator dateCalculator)
        {
            _dateCalculator = dateCalculator;
        }

        public IEnumerable<ProjectionEntry> Generate(
            FundingRule rule,
            DateOnly from,
            DateOnly to)
        {
            return Enumerable.Empty<ProjectionEntry>();
        }
    }
}
