using PlanAhead.Infrastructure.DB.SQLite;

public sealed class TestDatabase : IDisposable
{
    public SQLiteContext Context { get; }

    private readonly string _databasePath;

    public TestDatabase()
    {
        _databasePath =
            Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.db");

        Context = new SQLiteContext(_databasePath);
    }

    public void Dispose()
    {
        if (File.Exists(_databasePath))
            File.Delete(_databasePath);
    }
}