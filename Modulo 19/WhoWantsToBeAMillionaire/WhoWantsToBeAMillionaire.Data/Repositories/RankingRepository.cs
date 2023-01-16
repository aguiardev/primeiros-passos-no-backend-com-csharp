using Dapper;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Data.Repositories;

public class RankingRepository : IRankingRepository
{
    private readonly IConnection _connection;
    private const string _querySql = @"  
          SELECT *
            FROM (SELECT Award
                       , HelpCount + SkipCount as Help
                       , PlayerName
                       , HelpCount
                       , SkipCount
                    FROM RANKING)
        ORDER BY Award DESC, Help, PlayerName LIMIT 5";
    
    private const string _queryInsert = @"INSERT INTO RANKING
        (PLAYERNAME, HELPCOUNT, SKIPCOUNT, AWARD) VALUES
        (@PlayerName, @HelpCount, @SkipCount, @Award)";

    public RankingRepository(IConnection connection)
        => _connection = connection;

    public List<Ranking> GetAll()
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Query<Ranking>(_querySql).ToList();
        }
        catch (Exception ex)
        {
            return new List<Ranking>();
        }
    }

    public bool Create(Ranking ranking)
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