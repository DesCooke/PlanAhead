using SQLite;

namespace PlanAhead.Data.Database;

public sealed class SQLiteContext
{
    private SQLiteAsyncConnection? _connection;

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection != null)
            return _connection;

        var databasePath =
            Path.Combine(
                FileSystem.AppDataDirectory,
                "homebudget.db");

        _connection =
            new SQLiteAsyncConnection(databasePath);

        return _connection;
    }
}