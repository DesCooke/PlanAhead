using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.DB.SQLite
{
    using global::SQLite;
    using PlanAhead.Core.Logging;
    using PlanAhead.Core.Models.Domain;
    using PlanAhead.Infrastructure.DB.SQLite;

    public class LocalDatabaseService : ILocalDatabaseService
    {
        private readonly SQLiteContext _context;

        public LocalDatabaseService(SQLiteContext context)
        {
            _context = context;
        }

        public async Task DeleteDatabaseAsync()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                await _context.CloseAsync();

                if (File.Exists(_context.DatabasePath))
                    File.Delete(_context.DatabasePath);
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                throw;
            }
        }

        public async Task CreateDatabaseAsync()
        {
            using var log = MethodLoggingService.Begin();
            try
            {
                var db = await _context.GetConnectionAsync();
                await db.CreateTableAsync<Account>();
                await db.CreateTableAsync<Fund>();
            }
            catch (Exception ex)
            {
                log.Exception(ex);
                throw;
            }
        }
    }
}
