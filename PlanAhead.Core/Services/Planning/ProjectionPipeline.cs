using PlanAhead.Core.Interfaces.Services;
using PlanAhead.Core.Models.Projections;

public class ProjectionPipeline
{
    private readonly IEnumerable<IProjectionGenerator> _generators;

    public ProjectionPipeline(
        IEnumerable<IProjectionGenerator> generators)
    {
        _generators = generators;
    }

    public IEnumerable<ProjectionEntry> Generate(
        DateOnly from,
        DateOnly to)
    {
        return _generators
            .SelectMany(g => g.Generate(from, to))
            .OrderBy(e => e.Date);
    }
}