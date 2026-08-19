using SQLite;

namespace PlanAhead.Infrastructure.DB.SQLite;
public sealed class SQLiteContext
{
    public string DatabasePath { get; }

    private SQLiteAsyncConnection? _connection;

    public SQLiteContext(string databasePath)
    {
        DatabasePath = databasePath;
    }

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection == null)
            _connection = new SQLiteAsyncConnection(DatabasePath);

        return _connection;
    }

    public async Task CloseAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            _connection = null;
        }
    }
}