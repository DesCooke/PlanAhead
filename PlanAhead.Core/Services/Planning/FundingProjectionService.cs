using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;
using PlanAhead.Core.Services.Dates;
using PlanAhead.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Services.Planning
{
    public class FundingProjectionService : IFundingProjectionService
    {
        private readonly IDateCalculator _dateCalculator;

        public FundingProjectionService(
            IDateCalculator dateCalculator)
        {
            _dateCalculator = dateCalculator;
        }

        public IEnumerable<ProjectionEntry> Generate(
            FundingRule rule,
            DateOnly from,
            DateOnly to)
        {
            foreach (var date in _dateCalculator.GenerateOccurrences(rule, from, to))
            {
                yield return new ProjectionEntry
                {
                    AccountId = rule.AccountId,
                    FundId = rule.FundId,
                    Date = date,
                    Amount = rule.Amount,
                    Type = ProjectionType.Funding
                };
            }
        }
    }
}
