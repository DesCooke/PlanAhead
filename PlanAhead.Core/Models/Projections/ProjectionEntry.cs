using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Models.Projections;

public class ProjectionEntry
{
    public Guid AccountId { get; init; }

    public Guid? FundId { get; init; }

    public DateOnly Date { get; init; }

    public decimal Amount { get; init; }

    public ProjectionType Type { get; init; }

    public string Description { get; init; } = "";
}