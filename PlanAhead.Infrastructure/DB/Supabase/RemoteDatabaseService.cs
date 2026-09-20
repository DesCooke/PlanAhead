using Supabase;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.DB.Supabase
{
    using PlanAhead.Infrastructure.Authentication;
    using PlanAhead.Infrastructure.Logging;
    using PlanAhead.Infrastructure.Sync.Models;
    using Supabase;

    [MethodLogging]
    public class RemoteDatabaseService : IRemoteDatabaseService
    {
        private readonly Client _client;

        public RemoteDatabaseService(
            Client client)
        {
            _client = client;
        }

        public async Task DeleteUserDataAsync()
        {
            var result = await _client.Rpc(
                "clear_my_data",
                new Dictionary<string, object>());
        }
    }
}
