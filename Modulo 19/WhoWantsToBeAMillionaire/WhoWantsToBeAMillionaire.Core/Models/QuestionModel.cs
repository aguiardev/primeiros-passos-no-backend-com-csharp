using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Core.Models;

[ExcludeFromCodeCoverage]
public class QuestionModel
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public List<OptionsModel> Options { get; private set; }
    public int Number { get; set; }

    public QuestionModel(int id, string description, List<OptionsModel> options)
    {
        Id = id;
        Description = description;
        Options = options;
    }
}