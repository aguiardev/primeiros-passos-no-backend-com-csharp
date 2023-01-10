using Microsoft.Data.Sqlite;

namespace WhoWantsToBeAMillionaire.Data;

public interface IConnection
{
    SqliteConnection GetConnection();
}