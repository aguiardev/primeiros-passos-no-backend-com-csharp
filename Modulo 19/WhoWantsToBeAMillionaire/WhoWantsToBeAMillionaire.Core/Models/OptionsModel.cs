using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Core.Models;

[ExcludeFromCodeCoverage]
public class OptionsModel
{
    public int Number { get; set; }
    public string Description { get; set; }
    public bool IsCorrect { get; set; }
    public bool Hidden { get; set; } = false;

    public OptionsModel(int number, string description, bool isCorrect)
    {
        Number = number;
        Description = description;
        IsCorrect = isCorrect;
    }
}