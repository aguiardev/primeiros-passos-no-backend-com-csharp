namespace WhoWantsToBeAMillionaire.Core.Models;

public class QuestionModel
{
    public int Id { get; set; }
    public string Description { get; set; }
    public List<OptionsModel> Options { get; set; }
    public int Number { get; set; }

    public QuestionModel(int id, string description, List<OptionsModel> options)
    {
        Id = id;
        Description = description;
        Options = options;
    }
}