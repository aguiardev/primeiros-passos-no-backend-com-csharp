using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Core.Models;

[ExcludeFromCodeCoverage]
public class AwardsModel
{
    public int QuestionNumber { get; private set; }
    public int Correct { get; private set; }
    public int Stop { get; private set; }
    public int Wrong { get; private set; }

    public AwardsModel(int questioNumber, int correct, int stop, int wrong)
    {
        QuestionNumber = questioNumber;
        Correct = correct;
        Stop = stop;
        Wrong = wrong;
    }
}