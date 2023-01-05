﻿namespace WhoWantsToBeAMillionaire.App.Entities;

public class Question
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public List<Option> Options { get; set; }

    public Question(int id, string description, List<Option> options)
    {
        Id = id;
        Description = description;
        Options = options;
    }

    public void UpdateProps(Question question)
    {
        Id = question.Id;
        Number = question.Number;
        Description = question.Description;
        Options = question.Options;
    }
}