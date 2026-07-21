using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Projections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Services
{
    public interface ILedgerProjectionService
    {
        public IEnumerable<ProjectionEntry> Generate(
            IEnumerable<LedgerEntry> ledgerEntries,
            DateOnly from,
            DateOnly to);
    }
}
