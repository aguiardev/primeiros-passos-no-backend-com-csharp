namespace WhoWantsToBeAMillionaire.App.Events;

public class StartedArgs : EventArgs
{
    public string PlayerName;

    public StartedArgs(string playerName)
        => PlayerName = playerName;
}