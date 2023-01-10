namespace WhoWantsToBeAMillionaire.Core.Models;

public class OptionsModel
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public bool IsCorrect { get; set; }

    public OptionsModel(int number, string description)
    {
        Number = number;
        Description = description;
    }

    public OptionsModel(int number, string description, bool isCorrect)
    {
        Number = number;
        Description = description;
        IsCorrect = isCorrect;
    }
}