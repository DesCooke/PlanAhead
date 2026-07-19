using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Services
{
    public interface ILedgerProjectionService
    {
        IEnumerable<ProjectionEntry> Generate(
            FundingRule rule,
            DateOnly from,
            DateOnly to);
    }
}
