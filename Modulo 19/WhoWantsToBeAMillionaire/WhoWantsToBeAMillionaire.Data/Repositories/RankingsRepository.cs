using Dapper;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Data.Repositories;

public class RankingsRepository : IRankingsRepository
{
    private readonly IConnection _connection;
    private const string _querySql = @"  
          SELECT *
            FROM (SELECT Award
                       , HelpCount + SkipCount as Help
                       , PlayerName
                       , HelpCount
                       , SkipCount
                    FROM RANKINGS)
        ORDER BY Award DESC, Help, PlayerName LIMIT 5";
    
    private const string _queryInsert = @"INSERT INTO RANKINGS
        (PLAYERNAME, HELPCOUNT, SKIPCOUNT, AWARD) VALUES
        (@PlayerName, @HelpCount, @SkipCount, @Award)";

    public RankingsRepository(IConnection connection)
        => _connection = connection;

    public List<Rankings> GetAll()
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Query<Rankings>(_querySql).ToList();
        }
        catch (Exception ex)
        {
            return new List<Rankings>();
        }
    }

    public bool Create(Rankings ranking)
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Execute(_queryInsert, ranking) > 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}