using WhoWantsToBeAMillionaire.App.Entities;
using WhoWantsToBeAMillionaire.App.Events.Enums;

namespace WhoWantsToBeAMillionaire.App.Events;

public class QuestionAsweredArgs : EventArgs
{
    public QuestionAswered QuestionAswered;
    public decimal? Award;
    public Question? Question;
    public bool Stopped;

    public QuestionAsweredArgs()
    {

    }

    public QuestionAsweredArgs(QuestionAswered questionAswered)
    {
        QuestionAswered = questionAswered;
        Award = null;
        Question = null;
        Stopped = false;
    }

    public QuestionAsweredArgs(QuestionAswered questionAswered, Question question)
    {
        QuestionAswered = questionAswered;
        Award = null;
        Question = question;
        Stopped = false;
    }

    public QuestionAsweredArgs(QuestionAswered questionAswered, decimal award)
    {
        QuestionAswered = questionAswered;
        Award = award;
        Question = null;
        Stopped = false;
    }

    public static QuestionAsweredArgs CreateStopped(decimal award) => new QuestionAsweredArgs
    {
        QuestionAswered = QuestionAswered.None,
        Award = award,
        Question = null,
        Stopped = true
    };
}