using PlanAhead.Core.MethodLogging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Models.Projections;

[MethodLogging]
public class Forecast
{
    public List<ProjectionEntry> Entries { get; set;  } = new();

    public DateOnly From { get; init; }

    public DateOnly To { get; init; }
}