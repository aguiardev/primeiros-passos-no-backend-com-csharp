namespace WhoWantsToBeAMillionaire.App.Entities;

public class Question
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public List<Option> Options { get; set; }

    public Question(string description, List<Option> options)
    {
        Description = description;
        Options = options;
    }
}