using PlanAhead.Core.Models.Domain;
using PlanAhead.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Core.Interfaces.Services
{
    public interface IAccountHealthService
    {
        Task<Status> GetStatusAsync(Account account);
    }
}
