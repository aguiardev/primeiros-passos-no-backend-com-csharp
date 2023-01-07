namespace WhoWantsToBeAMillionaire.App.Entities;

public class Option
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public bool IsCorrect { get; set; }

    public Option(int number, string description)
    {
        Number = number;
        Description = description;
    }

    public Option(int number, string description, bool isCorrect)
    {
        Number = number;
        Description = description;
        IsCorrect = isCorrect;
    }
}