using System;
using System.Collections.Generic;
using System.Text;

namespace PlanAhead.Infrastructure.DB.SQLite
{
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
    }
}
