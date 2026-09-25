using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Models.Projections;

public class Forecast
{
    public List<ProjectionEntry> Entries { get; set;  } = new();

    public DateOnly From { get; init; }

    public DateOnly To { get; init; }
}