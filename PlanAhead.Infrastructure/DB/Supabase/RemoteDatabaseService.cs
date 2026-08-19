using System;
using System.Collections.Generic;
using System.Text;
using Supabase;

namespace PlanAhead.Infrastructure.DB.Supabase
{
    using PlanAhead.Infrastructure.Authentication;
    using PlanAhead.Infrastructure.Sync.Models;
    using Supabase;

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
