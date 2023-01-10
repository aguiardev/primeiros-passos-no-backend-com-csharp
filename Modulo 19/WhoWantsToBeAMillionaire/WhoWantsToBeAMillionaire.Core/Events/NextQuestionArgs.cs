﻿using WhoWantsToBeAMillionaire.Core.Models;

namespace WhoWantsToBeAMillionaire.Core.Events;

public class NextQuestionArgs : EventArgs
{
    public string PlayerName { get; private set; }
    public decimal CurrentAward { get; private set; }
    public QuestionModel Question { get; private set; }
    public AwardModel Award { get; private set; }
    public int SkipCount { get; private set; }
    public int HelpCount { get; private set; }
    public bool CallHelp { get; private set; }

    public NextQuestionArgs(
        string playerName,
        decimal currentAward,
        QuestionModel question,
        AwardModel award,
        int skipCount,
        int helpCount,
        bool callHelp)
    {
        PlayerName = playerName;
        CurrentAward = currentAward;
        Question = question;
        Award = award;
        SkipCount = skipCount;
        HelpCount = helpCount;
        CallHelp = callHelp;
    }
}