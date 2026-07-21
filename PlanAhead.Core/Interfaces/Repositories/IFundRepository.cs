using PlanAhead.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Repositories
{
    public interface IFundRepository
    {
        Task<Fund?> GetByIdAsync(Guid id);

        Task<List<Fund>> GetByAccountIdAsync(Guid accountId);

        Task AddAsync(Fund fund);

        Task UpdateAsync(Fund fund);

        Task DeleteAsync(Fund fund);
    }
}
