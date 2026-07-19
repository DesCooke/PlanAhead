using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Services
{
    public interface IFundingProjectionService
    {
        IEnumerable<ProjectionEntry> Generate(
            FundingRule rule,
            DateOnly from,
            DateOnly to);
    }
}
