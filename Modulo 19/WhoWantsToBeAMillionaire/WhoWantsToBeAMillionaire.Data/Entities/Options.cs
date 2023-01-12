namespace WhoWantsToBeAMillionaire.Data.Entities;

public class Options
{
    public int Id { get; set; }
    public string Description { get; set; }
    public bool Correct { get; set; }
    public int QuestionId { get; set; }
}