using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.DB.SQLite
{
    public interface ILocalDatabaseService
    {
        Task DeleteDatabaseAsync();
        Task CreateDatabaseAsync();
    }
}
