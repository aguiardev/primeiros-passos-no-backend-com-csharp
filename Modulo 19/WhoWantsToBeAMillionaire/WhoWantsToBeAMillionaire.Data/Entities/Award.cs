namespace WhoWantsToBeAMillionaire.Data.Entities;

public class Award
{
    public int Id { get; set; }
    public int Correct { get; set; }
    public int Stop { get; set; }
    public int Wrong { get; set; }
}