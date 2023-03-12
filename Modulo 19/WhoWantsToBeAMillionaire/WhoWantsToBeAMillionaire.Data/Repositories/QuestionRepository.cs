﻿using Dapper;
using WhoWantsToBeAMillionaire.Data.Entities;
using WhoWantsToBeAMillionaire.Data.Repositories.Interfaces;

namespace WhoWantsToBeAMillionaire.Data.Repositories;

public class QuestionRepository : IQuestionsRepository
{
    private readonly IConnection _connection;
    private const string _query = "SELECT ID, DESCRIPTION FROM QUESTION";

    public QuestionRepository(IConnection connection)
        => _connection = connection;

    public List<Questions> GetAll()
    {
        try
        {
            using var connection = _connection.GetConnection();
            connection.Open();

            return connection.Query<Questions>(_query).ToList();
        }
        catch (Exception ex)
        {
            return new List<Questions>();
        }
    }
}