using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Core.Models;

[ExcludeFromCodeCoverage]
public class AwardsModel
{
    public int QuestionNumber { get; private set; }
    public int RightAnswer { get; private set; }
    public int Stop { get; private set; }
    public int WrongAnswer { get; private set; }

    public AwardsModel(int questioNumber, int correct, int stop, int wrong)
    {
        QuestionNumber = questioNumber;
        RightAnswer = correct;
        Stop = stop;
        WrongAnswer = wrong;
    }
}