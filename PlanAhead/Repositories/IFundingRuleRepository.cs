using PlanAhead.Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Repositories
{
    public interface IFundingRuleRepository
    {
        Task<FundingRule?> GetByIdAsync(Guid id);

        Task<List<FundingRule>> GetByAccountIdAsync(Guid accountId);

        Task AddAsync(FundingRule rule);

        Task UpdateAsync(FundingRule rule);

        Task DeleteAsync(FundingRule rule);
    }
}
