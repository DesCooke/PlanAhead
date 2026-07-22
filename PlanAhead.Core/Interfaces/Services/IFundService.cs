using PlanAhead.Core.Models.Domain;

namespace PlanAhead.Core.Interfaces.Services;

public interface IFundService
{
    Task<List<Fund>> GetAllAsync();

    Task<List<Fund>> GetByAccountIdAsync(
        Guid accountId);

    Task<Fund?> GetByIdAsync(
        Guid id);

    Task AddAsync(
        Fund fund);

    Task UpdateAsync(
        Fund fund);

    Task DeleteAsync(
        Guid fundId);
}