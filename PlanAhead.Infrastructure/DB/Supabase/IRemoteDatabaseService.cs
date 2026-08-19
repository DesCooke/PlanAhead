using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.DB.Supabase
{
    public interface IRemoteDatabaseService
    {
        Task DeleteUserDataAsync();
    }
}
