using WhoWantsToBeAMillionaire.App.Enums;

namespace WhoWantsToBeAMillionaire.App.Events;

public class GameOverArgs : EventArgs
{
	public GameOverReason GameOverReason;
    public decimal Award;

    public GameOverArgs()
	{

	}

	public GameOverArgs(GameOverReason gameOverReason, decimal award)
	{
        GameOverReason = gameOverReason;
		Award = award;
    }
}