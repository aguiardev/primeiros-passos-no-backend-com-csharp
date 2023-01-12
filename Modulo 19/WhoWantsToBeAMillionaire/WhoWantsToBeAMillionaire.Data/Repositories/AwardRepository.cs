using Dapper;
using System.Diagnostics.CodeAnalysis;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Data.Repositories;

public class AwardRepository : IAwardRepository
{
    private readonly IConnection _connection;
    private const string _query = "SELECT ID, CORRECT, STOP, WRONG FROM AWARD";

    public AwardRepository(IConnection connection)
        => _connection = connection;

    public List<Award> GetAll()
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Query<Award>(_query).ToList();
        }
        catch (Exception ex)
        {
            return new List<Award>();
        }
    }
}