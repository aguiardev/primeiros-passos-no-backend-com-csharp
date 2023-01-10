using Dapper;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Data.Repositories;

public class OptionsRepository : IOptionsRepository
{
    private readonly IConnection _connection;
    private const string _query = "SELECT ID, DESCRIPTION, CORRECT, QUESTION_ID AS QUESTIONID FROM OPTIONS";

    public OptionsRepository(IConnection connection)
        => _connection = connection;

    public List<Options> GetAll()
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Query<Options>(_query).ToList();
        }
        catch (Exception ex)
        {
            return new List<Options>();
        }
    }
}