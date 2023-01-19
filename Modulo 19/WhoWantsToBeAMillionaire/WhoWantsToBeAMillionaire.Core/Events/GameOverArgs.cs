using WhoWantsToBeAMillionaire.Core.Enums;

namespace WhoWantsToBeAMillionaire.Core.Events;

public class GameOverArgs : EventArgs
{
	public GameOverReason GameOverReason;
    public decimal Award;

	public GameOverArgs(GameOverReason gameOverReason, decimal award)
	{
        GameOverReason = gameOverReason;
		Award = award;
    }
}