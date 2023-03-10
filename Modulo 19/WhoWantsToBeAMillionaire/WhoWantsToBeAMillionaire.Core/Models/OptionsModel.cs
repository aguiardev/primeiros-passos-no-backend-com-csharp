using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Core.Models;

[ExcludeFromCodeCoverage]
public class OptionsModel
{
    public int Number { get; set; }
    public string Description { get; private set; }
    public bool IsCorrect { get; private set; }
    public bool Hidden { get; set; } = false;

    public OptionsModel(int number, string description, bool isCorrect)
    {
        Number = number;
        Description = description;
        IsCorrect = isCorrect;
    }
}