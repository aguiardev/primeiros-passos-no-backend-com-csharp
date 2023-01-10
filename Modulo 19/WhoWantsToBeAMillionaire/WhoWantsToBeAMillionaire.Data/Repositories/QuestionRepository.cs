using Dapper;
using System.Diagnostics.CodeAnalysis;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Data.Repositories;

[ExcludeFromCodeCoverage]
public class QuestionRepository : IQuestionRepository
{
    private readonly IConnection _connection;
    private const string _query = "SELECT ID, DESCRIPTION FROM QUESTION";

    public QuestionRepository(IConnection connection)
        => _connection = connection;

    public List<Question> GetAll()
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Query<Question>(_query).ToList();
        }
        catch (Exception ex)
        {
            return new List<Question>();
        }
    }
}