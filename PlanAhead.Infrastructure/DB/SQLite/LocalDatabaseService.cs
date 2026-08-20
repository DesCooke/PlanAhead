using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.DB.SQLite
{
    using global::SQLite;
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
            await _context.CloseAsync();

            if (File.Exists(_context.DatabasePath))
                File.Delete(_context.DatabasePath);
        }

        public async Task CreateDatabaseAsync()
        {
            var db = await _context.GetConnectionAsync();
            await db.CreateTableAsync<Account>();
            await db.CreateTableAsync<Fund>();
        }
    }
}
