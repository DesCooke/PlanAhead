namespace PlanAhead.Core.Interfaces.Services;

public interface IApplicationStartupService
{
    Task<Page> GetStartupPageAsync();
}