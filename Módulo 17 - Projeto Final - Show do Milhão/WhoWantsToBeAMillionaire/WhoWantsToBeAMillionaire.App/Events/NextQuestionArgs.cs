using WhoWantsToBeAMillionaire.App.Entities;

namespace WhoWantsToBeAMillionaire.App.Events;

public class NextQuestionArgs : EventArgs
{
    public string QuestionNumber { get; private set; }
    public Question Question { get; private set; }
    public string PlayerName { get; private set; }
    public decimal CurrentAward { get; private set; }
    public int SkipCount { get; private set; }
    public int HelpCount { get; private set; }
    public Award Award { get; private set; }

    public NextQuestionArgs(string questionNumber, Question question, string playerName, decimal currentAward, int skipCount, int helpCount, Award award)
    {
        QuestionNumber = questionNumber;
        Question = question;
        PlayerName = playerName;
        CurrentAward = currentAward;
        SkipCount = skipCount;
        HelpCount = helpCount;
        Award = award;
    }
}