using System.Diagnostics.CodeAnalysis;

namespace WhoWantsToBeAMillionaire.Core.Models;

[ExcludeFromCodeCoverage]
public class QuestionsModel
{
    public int Id { get; private set; }
    public string Description { get; private set; }
    public List<OptionsModel> Options { get; private set; }
    public int Number { get; set; }

    public QuestionsModel(int id, string description, List<OptionsModel> options)
    {
        Id = id;
        Description = description;
        Options = options;
    }
}