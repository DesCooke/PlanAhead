using SQLite;

namespace PlanAhead.Infrastructure.Data.Database;

public sealed class SQLiteContext
{
    private readonly string _databasePath;

    private SQLiteAsyncConnection? _connection;

    public SQLiteContext(string databasePath)
    {
        _databasePath = databasePath;
    }

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection != null)
            return _connection;

        _connection = new SQLiteAsyncConnection(_databasePath);

        return _connection;
    }
}