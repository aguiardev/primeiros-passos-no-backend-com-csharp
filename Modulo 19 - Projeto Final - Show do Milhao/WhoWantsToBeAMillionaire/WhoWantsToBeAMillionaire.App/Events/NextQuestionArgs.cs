using WhoWantsToBeAMillionaire.App.Entities;

namespace WhoWantsToBeAMillionaire.App.Events;

public class NextQuestionArgs : EventArgs
{
    public Question Question { get; private set; }
    public string PlayerName { get; private set; }
    public decimal CurrentAward { get; private set; }
    public int SkipCount { get; private set; }
    public bool CallHelp { get; private set; }
    public int HelpCount { get; private set; }
    public Award Award { get; private set; }

    public NextQuestionArgs(
        Question question,
        string playerName,
        decimal currentAward,
        int skipCount,
        bool callHelp,
        int helpCount,
        Award award)
    {
        Question = question;
        PlayerName = playerName;
        CurrentAward = currentAward;
        SkipCount = skipCount;
        CallHelp = callHelp;
        HelpCount = helpCount;
        Award = award;
    }
}