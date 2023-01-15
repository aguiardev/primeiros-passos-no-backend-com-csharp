namespace WhoWantsToBeAMillionaire.Data.Entities;

public class Ranking
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int HelpCount { get; set; }
    public int SkipCount { get; set; }
    public int Award { get; set; }
}