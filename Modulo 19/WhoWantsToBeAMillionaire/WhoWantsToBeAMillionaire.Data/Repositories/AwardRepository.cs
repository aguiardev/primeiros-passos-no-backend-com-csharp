using Dapper;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Data.Repositories;

public class AwardRepository : IAwardsRepository
{
    private readonly IConnection _connection;
    private const string _query = "SELECT ID, CORRECT, STOP, WRONG FROM AWARD";

    public AwardRepository(IConnection connection)
        => _connection = connection;

    public List<Awards> GetAll()
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Query<Awards>(_query).ToList();
        }
        catch (Exception ex)
        {
            return new List<Awards>();
        }
    }
}