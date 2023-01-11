using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Core.Models;

[ExcludeFromCodeCoverage]
public class AwardModel
{
    public int QuestionNumber { get; private set; }
    public decimal Correct { get; private set; }
    public decimal Stop { get; private set; }
    public decimal Wrong { get; private set; }

    public AwardModel(int questioNumber, decimal correct, decimal stop, decimal wrong)
    {
        QuestionNumber = questioNumber;
        Correct = correct;
        Stop = stop;
        Wrong = wrong;
    }
}