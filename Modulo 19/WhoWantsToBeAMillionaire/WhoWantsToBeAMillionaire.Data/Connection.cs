using Microsoft.Data.Sqlite;

namespace WhoWantsToBeAMillionaire.Data;

public class Connection : IConnection
{
    private readonly string _connectionString;

    public Connection(string connectionString)
        => _connectionString = connectionString;

    public SqliteConnection GetConnection() => new(_connectionString);
}