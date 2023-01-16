namespace WhoWantsToBeAMillionaire.Core.Models;

public class RankingModel
{
    public string PlayerName { get; set; }
    public int HelpCount { get; set; }
    public int SkipCount { get; set; }
    public int Award { get; set; }

    public RankingModel(string playerName, int helpCount, int skipCount, int award)
    {
        PlayerName = playerName;
        HelpCount = helpCount;
        SkipCount = skipCount;
        Award = award;
    }

    public string ToString(
        int widthPlayerName, int widthHelpCount, int widthSkipCount, int widthAward)
    {
        var template = "|{0}|{1}|{2}|{3}|";

        return string.Format(
            template,
            PlayerName.PadRight(widthPlayerName),
            HelpCount.ToString().PadRight(widthHelpCount),
            SkipCount.ToString().PadRight(widthSkipCount),
            Award.ToString().PadRight(widthAward));
    }
}