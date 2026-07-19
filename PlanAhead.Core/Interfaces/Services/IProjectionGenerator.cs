using System;
using System.Collections.Generic;
using System.Text;

using PlanAhead.Core.Models.Projections;

namespace PlanAhead.Core.Interfaces.Services;

public interface IProjectionGenerator
{
    IEnumerable<ProjectionEntry> Generate(
        DateOnly from,
        DateOnly to);
}