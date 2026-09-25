using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Models.Projections
{
    public class ProjectedFunding
    {
        public DateOnly Date { get; init; }

        public decimal Amount { get; init; }

        public Guid FundId { get; init; }

        public Guid AccountId { get; init; }
    }
}
